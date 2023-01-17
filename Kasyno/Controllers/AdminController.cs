#pragma warning disable CS8601, CS8604, CS8618, CS8602

using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Models.CoinFlip;
using Models.TokenVerification;
using Models.UserModel;

namespace Kasyno.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AdminController : ControllerBase
{
    private string connectionString;
    private static readonly HttpClient client = new HttpClient();
    private UserController userController;
    public AdminController(UserController userController, IConfiguration _configuration)
    {
        this.connectionString = _configuration.GetConnectionString("Database");
        this.userController = userController;
    }
    [HttpPost("getallusers")]
    public IActionResult GetAllUsers([FromBody] TokenVerification tokenVerification)
    {
        if (!ModelState.IsValid) return Ok(new { statusCode = false, message = ModelState });
        JsonResult answer = userController.GetUser(new TokenVerification(tokenVerification.id, tokenVerification.token));
        User? user = answer.Value as User;
        if (user?.id == 0 || user?.admin == false) return Ok(new { statusCode = false, message = "You are not permited to do this action" });
        else
        {
            var listOfUsers = new List<UserFull>();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("GetAllUsers", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader r = cmd.ExecuteReader();
                    while (r.Read())
                    {
                        UserFull userTemp = new UserFull(Convert.ToInt32(r["Id"]), r["Login"].ToString(), r["Email"].ToString(), Convert.ToBoolean(r["Admin"]), Convert.ToInt32(r["Money"]), Convert.ToBoolean(r["Active"]), Convert.ToBoolean(r["Banned"]));
                        listOfUsers.Add(userTemp);
                    }
                    con.Close();
                }
                return Ok(new { statusCode = true, message = listOfUsers });
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message); // logger
                return Ok(new { statusCode = false, message = "You are not permited to do this action" });
            }
        }
    }
}