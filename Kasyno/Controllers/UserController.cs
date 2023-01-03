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
    [HttpGet("login")]
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
                    var token = JwtSecurityToken(userRequest);
                    return Ok(new { statusCode = true, message = token });
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
}