#pragma warning disable CS8601, CS8604, CS8618

using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Models.UserModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using AuthenticationJWT;
using System.Text;
using Models.TokenVerification;

namespace Kasyno.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private string connectionString;
    private AuthenticationJwtSettings authenticationJwtSettings;
    public UserController(IConfiguration _configuration, AuthenticationJwtSettings authenticationJwtSettings)
    {
        this.connectionString = _configuration.GetConnectionString("Database");
        this.authenticationJwtSettings = authenticationJwtSettings;
    }
    [HttpPost("register")]
    public IActionResult Register([FromBody] UserRegistration user)
    {
        if (!ModelState.IsValid) return BadRequest(new { statusCode = false, message = ModelState });
        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("InsertUser", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@login", user.login);
                cmd.Parameters.AddWithValue("@password", BCrypt.Net.BCrypt.HashPassword(user.password));
                cmd.Parameters.AddWithValue("@email", user.email);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            return Ok(new { statusCode = true, message = "Account created" });
        }
        catch (SqlException e)
        {
            if (e.Number == 2627)
            {
                if (Check("Users_CheckLogin", "@login", user.login)) return Ok(new { statusCode = false, message = "This login is used" });
                if (Check("Users_CheckEmail", "@email", user.email)) return Ok(new { statusCode = false, message = "This email is used" });
            }
            return BadRequest(new { statusCode = false, message = "Error: " + e.Message });
        }
    }
    [HttpPost("login")]
    public IActionResult Login([FromBody] UserLogin user)
    {
        if (!ModelState.IsValid) return BadRequest(new { statusCode = false, message = ModelState });
        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetUser", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@login", user.login);
                con.Open();
                SqlDataReader r = cmd.ExecuteReader();
                if (!r.Read()) return Ok(new { statusCode = false, message = "Login do not match any account" });
                if (BCrypt.Net.BCrypt.Verify(user.password, r["Password"].ToString()))
                {
                    if (!Convert.ToBoolean(r["Active"])) return Ok(new { statusCode = false, message = "This account is not active" });
                    if (Convert.ToBoolean(r["Banned"])) return Ok(new { statusCode = false, message = "This account is banned" });
                    User userRequest = new User(Convert.ToInt32(r["Id"]), r["Login"].ToString(), r["Email"].ToString(), Convert.ToBoolean(r["Admin"]), Convert.ToInt32(r["Money"]));
                    var tokenRequest = JwtSecurityToken(userRequest);
                    if (UpdateToken(userRequest.id, tokenRequest)) Ok(new { statusCode = false, message = "Server do not work. Please contact to administrator!" });
                    return Ok(new { statusCode = true, message = new { token = tokenRequest, user = userRequest } });
                }
                else return Ok(new { statusCode = false, message = "Incorrect password" });
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.Message); // logger
            return BadRequest(new { statusCode = false, message = "Error " + e.Message });
        }
    }
    private bool Check(string storedProcedure, string indicator, string value)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(storedProcedure, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue(indicator, value);
                con.Open();
                SqlDataReader r = cmd.ExecuteReader();
                if (r.Read()) return true;
                con.Close();
            }
            return false;
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.Message); // logger
            return false;
        }
    }
    private string JwtSecurityToken(User user)
    {
        var claimUser = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
            new Claim(ClaimTypes.Name, user.login.ToString()),
            new Claim(ClaimTypes.Role, user.admin.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationJwtSettings.JwtKey));
        var cred = new SigningCredentials(key, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256);

        var expiresDate = DateTime.Now.AddDays(authenticationJwtSettings.JwtExpireDays);

        var token = new JwtSecurityToken(authenticationJwtSettings.JwtIssuer,
        authenticationJwtSettings.JwtIssuer, claimUser, expires: expiresDate, signingCredentials: cred);

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }
    private bool UpdateToken(int id, string token)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SetToken", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@token", token);
                con.Open();
                SqlDataReader r = cmd.ExecuteReader();
                if (!r.Read()) return false;
                con.Close();
            }
            return true;
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.Message); // logger
            return false;
        }
    }
    [HttpPost("getuser")]
    public JsonResult GetUser([FromBody] TokenVerification tokenVerification)
    {
        if (tokenVerification.token != null) tokenVerification.token = tokenVerification.token.Trim(new Char[] { '"' });
        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("VerifyUser", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", tokenVerification.id);
                cmd.Parameters.AddWithValue("@token", tokenVerification.token);
                con.Open();
                SqlDataReader r = cmd.ExecuteReader();
                if (!r.Read()) return new JsonResult(new User(0, "", "", false, 0));
                if (!Convert.ToBoolean(r["Active"])) return new JsonResult(new User(0, "", "", false, 0));
                if (Convert.ToBoolean(r["Banned"])) return new JsonResult(new User(0, "", "", false, 0));
                User userRequest = new User(Convert.ToInt32(r["Id"]), r["Login"].ToString(), r["Email"].ToString(), Convert.ToBoolean(r["Admin"]), Convert.ToInt32(r["Money"]));
                return new JsonResult(userRequest);
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.Message); // logger
            return new JsonResult(new User(0, "", "", false, 0));
        }
    }
    [HttpPost("deleteuser")]
    public IActionResult DeleteUser([FromBody] TokenVerificationWithPassword tokenVerificationWithPassword)
    {
        if (!ModelState.IsValid) return BadRequest(new { statusCode = false, message = ModelState });
        if (tokenVerificationWithPassword.token != null) tokenVerificationWithPassword.token = tokenVerificationWithPassword.token.Trim(new Char[] { '"' });
        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("VerifyUser", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", tokenVerificationWithPassword.id);
                cmd.Parameters.AddWithValue("@token", tokenVerificationWithPassword.token);
                con.Open();
                SqlDataReader r = cmd.ExecuteReader();
                if (!r.Read()) return Ok(new { statusCode = false, message = "You are not permited to do this action. Please login again" });
                if (BCrypt.Net.BCrypt.Verify(tokenVerificationWithPassword.password, r["Password"].ToString()))
                {
                    if (!Convert.ToBoolean(r["Active"])) return Ok(new { statusCode = false, message = "This account is not active, cannot be removed from database" });
                    if (Convert.ToBoolean(r["Banned"])) return Ok(new { statusCode = false, message = "This account is banned, cannot be removed from database" });
                    if (DeleteUserContinue(tokenVerificationWithPassword.id)) Ok(new { statusCode = false, message = "Something went wrong, please try later" });
                    return Ok(new { statusCode = true, message = "Account has been deleted" });
                }
                else return Ok(new { statusCode = false, message = "Incorrect password" });
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.Message); // logger
            return BadRequest(new { statusCode = false, message = "Error " + e.Message });
        }
    }
    private bool DeleteUserContinue(int id)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("RemoveUser", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                SqlDataReader r = cmd.ExecuteReader();
                if (!r.Read()) return false;
                return true;
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.Message); // logger
            return false;
        }
    }
    [HttpPost("rechargeaccount")]
    public IActionResult ChargeAccount([FromBody] TokenVerificationWithAccountCharge tokenVerificationWithAccountCharge)
    {
        if (!ModelState.IsValid) return BadRequest(new { statusCode = false, message = ModelState });
        if (tokenVerificationWithAccountCharge.token != null) tokenVerificationWithAccountCharge.token = tokenVerificationWithAccountCharge.token.Trim(new Char[] { '"' });
        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("VerifyUser", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", tokenVerificationWithAccountCharge.id);
                cmd.Parameters.AddWithValue("@token", tokenVerificationWithAccountCharge.token);
                con.Open();
                SqlDataReader r = cmd.ExecuteReader();
                if (!r.Read()) return Ok(new { statusCode = false, message = "You are not permited to do this action. Please login again" });
                if (!Convert.ToBoolean(r["Active"])) return Ok(new { statusCode = false, message = "This account is not active, cannot be charged" });
                if (Convert.ToBoolean(r["Banned"])) return Ok(new { statusCode = false, message = "This account is banned, cannot be charged" });
                if (ChargeAccountContinue(tokenVerificationWithAccountCharge.id, tokenVerificationWithAccountCharge.value)) Ok(new { statusCode = false, message = "Something went wrong, please try later" });
                return Ok(new { statusCode = true, message = "Account has been charged successfuly" });
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.Message); // logger
            return BadRequest(new { statusCode = false, message = "Error " + e.Message });
        }
    }
    private bool ChargeAccountContinue(int id, int value)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("UpdateMoney", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@value", value);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                return true;
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.Message); // logger
            return false;
        }
    }
    [HttpPost("changepassword")]
    public IActionResult ChangePassword([FromBody] TokenVerificationWithPasswordChange tokenVerificationWithPasswordChange)
    {
        if (!ModelState.IsValid) return BadRequest(new { statusCode = false, message = ModelState });
        if (tokenVerificationWithPasswordChange.token != null) tokenVerificationWithPasswordChange.token = tokenVerificationWithPasswordChange.token.Trim(new Char[] { '"' });
        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("VerifyUser", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", tokenVerificationWithPasswordChange.id);
                cmd.Parameters.AddWithValue("@token", tokenVerificationWithPasswordChange.token);
                con.Open();
                SqlDataReader r = cmd.ExecuteReader();
                if (!r.Read()) return Ok(new { statusCode = false, message = "You are not permited to do this action. Please login again" });
                if (BCrypt.Net.BCrypt.Verify(tokenVerificationWithPasswordChange.oldPassword, r["Password"].ToString()))
                {
                    if (!Convert.ToBoolean(r["Active"])) return Ok(new { statusCode = false, message = "This account is not active, cannot change password" });
                    if (Convert.ToBoolean(r["Banned"])) return Ok(new { statusCode = false, message = "This account is banned, cannot change password" });
                    if (ChangePasswordContinue(tokenVerificationWithPasswordChange.id, tokenVerificationWithPasswordChange.newPassword)) Ok(new { statusCode = false, message = "Something went wrong, please try later" });
                    
                    // tutaj
                    
                    return Ok(new { statusCode = true, message = "Password has been changed" });
                }
                else return Ok(new { statusCode = false, message = "Incorrect password" });
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.Message); // logger
            return BadRequest(new { statusCode = false, message = "Error " + e.Message });
        }
    }
    private bool ChangePasswordContinue(int id, string password)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("UpdatePassword", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@password", password);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                return true;
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.Message); // logger
            return false;
        }
    }
}