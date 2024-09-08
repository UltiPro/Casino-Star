using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Models.UserModel;
using Models.TokenVerification;
using AuthenticationJWT;

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
            return Ok(new { statusCode = true, message = "Account created." });
        }
        catch (SqlException e)
        {
            if (e.Number == 2627)
            {
                if (Check("Users_CheckLogin", "@login", user.login)) return Unauthorized(new { statusCode = false, message = "This login is used." });
                if (Check("Users_CheckEmail", "@email", user.email)) return Unauthorized(new { statusCode = false, message = "This email is used." });
            }
            return UnprocessableEntity(new { statusCode = false, message = "Something went wrong, please try later." });
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
                if (!r.Read()) return NotFound(new { statusCode = false, message = "Login do not match any account." });
                if (BCrypt.Net.BCrypt.Verify(user.password, r["Password"].ToString()))
                {
                    if (!Convert.ToBoolean(r["Active"])) return Unauthorized(new { statusCode = false, message = "This account is not active." });
                    if (Convert.ToBoolean(r["Banned"])) return Unauthorized(new { statusCode = false, message = "This account is banned." });
                    User userRequest = new User(Convert.ToInt32(r["Id"]), r["Login"].ToString(), r["Email"].ToString(), Convert.ToBoolean(r["Admin"]), Convert.ToInt32(r["Money"]));
                    var tokenRequest = JwtSecurityToken(userRequest);
                    if (UpdateToken(userRequest.id, tokenRequest)) UnprocessableEntity(new { statusCode = false, message = "Something went wrong, please try later." });
                    return Ok(new { statusCode = true, message = new { token = tokenRequest, user = userRequest } });
                }
                else return Ok(new { statusCode = false, message = "Incorrect password." });
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.Message); // logger
            return UnprocessableEntity(new { statusCode = false, message = "Something went wrong, please try later." });
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message); // logger
            return UnprocessableEntity(new { statusCode = false, message = "Something went wrong, please try later." });
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
        catch (Exception e)
        {
            Console.WriteLine(e.Message); // logger
            throw e;
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
        catch (Exception e)
        {
            Console.WriteLine(e.Message); // logger
            throw e;
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
        catch (Exception e)
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
                if (!r.Read()) return Unauthorized(new { statusCode = false, message = "You are not permited to do this action. Please login again." });
                if (BCrypt.Net.BCrypt.Verify(tokenVerificationWithPassword.password, r["Password"].ToString()))
                {
                    if (!Convert.ToBoolean(r["Active"])) return Unauthorized(new { statusCode = false, message = "This account is not active, cannot be removed from database." });
                    if (Convert.ToBoolean(r["Banned"])) return Unauthorized(new { statusCode = false, message = "This account is banned, cannot be removed from database." });
                    if (DeleteUserContinue(tokenVerificationWithPassword.id)) UnprocessableEntity(new { statusCode = false, message = "Something went wrong, please try later." });
                    return Ok(new { statusCode = true, message = "Account has been deleted." });
                }
                else return Ok(new { statusCode = false, message = "Incorrect password." });
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.Message); // logger
            return UnprocessableEntity(new { statusCode = false, message = "Something went wrong, please try later." });
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message); // logger
            return UnprocessableEntity(new { statusCode = false, message = "Something went wrong, please try later." });
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
                cmd.ExecuteNonQuery();
                return true;
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.Message); // logger
            return false;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message); // logger
            throw e;
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
                if (!r.Read()) return Unauthorized(new { statusCode = false, message = "You are not permited to do this action. Please login again." });
                if (!Convert.ToBoolean(r["Active"])) return Unauthorized(new { statusCode = false, message = "This account is not active, cannot be charged." });
                if (Convert.ToBoolean(r["Banned"])) return Unauthorized(new { statusCode = false, message = "This account is banned, cannot be charged." });
                if (ChargeAccountContinue(tokenVerificationWithAccountCharge.id, tokenVerificationWithAccountCharge.value)) UnprocessableEntity(new { statusCode = false, message = "Something went wrong, please try later." });
                return Ok(new { statusCode = true, message = "Account has been charged successfuly." });
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.Message); // logger
            return UnprocessableEntity(new { statusCode = false, message = "Something went wrong, please try later." });
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message); // logger
            return UnprocessableEntity(new { statusCode = false, message = "Something went wrong, please try later." });
        }
    }

    public bool ChargeAccountContinue(int id, int value)
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
                return true;
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.Message); // logger
            return false;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message); // logger
            throw e;
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
                if (!r.Read()) return Unauthorized(new { statusCode = false, message = "You are not permited to do this action. Please login again." });
                if (BCrypt.Net.BCrypt.Verify(tokenVerificationWithPasswordChange.oldPassword, r["Password"].ToString()))
                {
                    if (!Convert.ToBoolean(r["Active"])) return Unauthorized(new { statusCode = false, message = "This account is not active, cannot change password." });
                    if (Convert.ToBoolean(r["Banned"])) return Unauthorized(new { statusCode = false, message = "This account is banned, cannot change password." });
                    if (ChangePasswordContinue(tokenVerificationWithPasswordChange.id, tokenVerificationWithPasswordChange.newPassword)) UnprocessableEntity(new { statusCode = false, message = "Something went wrong, please try later." });
                    return Ok(new { statusCode = true, message = "Password has been changed. Please login again to have fully access on this service." });
                }
                else return Ok(new { statusCode = false, message = "Incorrect old password." });
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.Message); // logger
            return UnprocessableEntity(new { statusCode = false, message = "Something went wrong, please try later." });
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message); // logger
            return UnprocessableEntity(new { statusCode = false, message = "Something went wrong, please try later." });
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
                cmd.Parameters.AddWithValue("@password", BCrypt.Net.BCrypt.HashPassword(password));
                con.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.Message); // logger
            return false;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message); // logger
            throw e;
        }
    }

    [HttpPost("changeemail")]
    public IActionResult ChangeEmail([FromBody] TokenVerificationWithEmailChange tokenVerificationWithEmailChange)
    {
        if (!ModelState.IsValid) return BadRequest(new { statusCode = false, message = ModelState });
        if (tokenVerificationWithEmailChange.token != null) tokenVerificationWithEmailChange.token = tokenVerificationWithEmailChange.token.Trim(new Char[] { '"' });
        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("VerifyUser", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", tokenVerificationWithEmailChange.id);
                cmd.Parameters.AddWithValue("@token", tokenVerificationWithEmailChange.token);
                con.Open();
                SqlDataReader r = cmd.ExecuteReader();
                if (!r.Read()) return Unauthorized(new { statusCode = false, message = "You are not permited to do this action. Please login again." });
                if (!Convert.ToBoolean(r["Active"])) return Unauthorized(new { statusCode = false, message = "This account is not active, cannot change email." });
                if (Convert.ToBoolean(r["Banned"])) return Unauthorized(new { statusCode = false, message = "This account is banned, cannot change email." });
                if (!Convert.ToString(r["Email"]).Equals(tokenVerificationWithEmailChange.oldEmail)) return Ok(new { statusCode = false, message = "Wrong old email." });
                if (ChangeEmailContinue(tokenVerificationWithEmailChange.id, tokenVerificationWithEmailChange.newEmail)) UnprocessableEntity(new { statusCode = false, message = "Something went wrong, please try later." });
                return Ok(new { statusCode = true, message = "Email has been changed." });
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.Message); // logger
            return UnprocessableEntity(new { statusCode = false, message = "Something went wrong, please try later." });
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message); // logger
            return UnprocessableEntity(new { statusCode = false, message = "Something went wrong, please try later." });
        }
    }

    private bool ChangeEmailContinue(int id, string email)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("UpdateEmail", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@email", email);
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
        catch (Exception e)
        {
            Console.WriteLine(e.Message); // logger
            throw e;
        }
    }
}