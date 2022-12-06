#pragma warning disable CS8601, CS8604, CS8618

using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Models.UserModel;
using Models.UserDataTests;

namespace Kasyno.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private string connectionString;
    public UserController(IConfiguration _configuration)
    {
        this.connectionString = _configuration.GetConnectionString("Database");
    }
    [HttpPost]
    public JsonResult Post(UserRegistration userRegistration)
    {
        if (!UserTests.LoginTest(userRegistration.login)) return new JsonResult(new { statusCode = false, message = "Validation failed at login" });
        if (!UserTests.EmailTest(userRegistration.email)) return new JsonResult(new { statusCode = false, message = "Validation failed at emial" });
        if (!UserTests.PasswordTest(userRegistration.password)) return new JsonResult(new { statusCode = false, message = "Validation failed at password" });
        if (!UserTests.PasswordTest(userRegistration.c_password)) return new JsonResult(new { statusCode = false, message = "Validation failed at confirm password" });
        if (!UserTests.MatchPasswords(userRegistration.password, userRegistration.c_password)) return new JsonResult(new { statusCode = false, message = "Validation failed at match passwords" });
        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("InsertUser", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@login", userRegistration.login);
                cmd.Parameters.AddWithValue("@password", BCrypt.Net.BCrypt.HashPassword(userRegistration.password));
                cmd.Parameters.AddWithValue("@email", userRegistration.email);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            return new JsonResult(new { statusCode = true, message = "Account created" });
        }
        catch (SqlException e)
        {
            if (e.Number == 2627)
            {
                if (Check("Users_CheckLogin", "@login", userRegistration.login)) return new JsonResult(new { statusCode = false, message = "This login is used" });
                if (Check("Users_CheckEmail", "@email", userRegistration.email)) return new JsonResult(new { statusCode = false, message = "This email is used" });
            }
            return new JsonResult(new { statusCode = false, message = "Error: " + e.Message });
        }
    }
    public JsonResult Get(UserLogin userLogin)
    {
        if (!UserTests.LoginTest(userLogin.login)) return new JsonResult(new { statusCode = false, message = "Validation failed at login" });
        if (!UserTests.PasswordTest(userLogin.password)) return new JsonResult(new { statusCode = false, message = "Validation failed at password" });
        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("Users_GetUser", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@login", userLogin.login);
                con.Open();
                SqlDataReader r = cmd.ExecuteReader();
                if (!r.Read()) return new JsonResult(new { statusCode = false, message = "Login do not match any account" });
                if (BCrypt.Net.BCrypt.Verify(userLogin.password, r["Password"].ToString()))
                {
                    if (!Convert.ToBoolean(r["Active"])) return new JsonResult(new { statusCode = false, message = "This account is not active" });
                    if (Convert.ToBoolean(r["Banned"])) return new JsonResult(new { statusCode = false, message = "This account is banned" });
                    User user = new User(Convert.ToInt32(r["Id"]), r["Login"].ToString(), r["Email"].ToString(), Convert.ToBoolean(r["Admin"]), Convert.ToInt32(r["Money"]));
                    return new JsonResult(new { statusCode = true, message = user });
                }
                else return new JsonResult(new { statusCode = false, message = "Incorrect password" });
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.Message); // logger
            return new JsonResult(new { statusCode = false, message = "Error " + e.Message });
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
}