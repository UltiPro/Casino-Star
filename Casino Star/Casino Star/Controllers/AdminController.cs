#pragma warning disable CS8601, CS8604, CS8618

using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Models.TokenVerification;
using Models.UserModel;
using Models.RequestAdmin;
using Models.RequestAdminMoney;

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
        if (!ModelState.IsValid) return BadRequest(new { statusCode = false, message = ModelState });
        JsonResult answer = userController.GetUser(new TokenVerification(tokenVerification.id, tokenVerification.token));
        User? user = answer.Value as User;
        if (user?.id == 0) return Unauthorized(new { statusCode = false, message = "You are not permited to do this action, please re-login!" });
        if (user?.admin == false) return Unauthorized(new { statusCode = false, message = "You are not permited to do this action, you are not admin!" });
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
                return UnprocessableEntity("Sql Error: " + e.Message + ".");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); // logger
                return UnprocessableEntity("Other Error: " + e.Message + ".");
            }
        }
    }

    [HttpPost("banuser")]
    public bool BanUser([FromBody] RequestAdmin requestAdmin)
    {
        if (!ModelState.IsValid) return false;
        JsonResult answer = userController.GetUser(new TokenVerification(requestAdmin.idAdmin, requestAdmin.token));
        User? user = answer.Value as User;
        if (user?.id == 0 || user?.admin == false) return false;
        else
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("UpdateBanned", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", requestAdmin.idTarget);
                    con.Open();
                    cmd.ExecuteNonQuery();
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
                return false;
            }
        }
    }

    [HttpPost("activeuser")]
    public bool ActiveUser([FromBody] RequestAdmin requestAdmin)
    {
        if (!ModelState.IsValid) return false;
        JsonResult answer = userController.GetUser(new TokenVerification(requestAdmin.idAdmin, requestAdmin.token));
        User? user = answer.Value as User;
        if (user?.id == 0 || user?.admin == false) return false;
        else
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("UpdateActive", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", requestAdmin.idTarget);
                    con.Open();
                    cmd.ExecuteNonQuery();
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
                return false;
            }
        }
    }

    [HttpPost("adminuser")]
    public bool AdminUser([FromBody] RequestAdmin requestAdmin)
    {
        if (!ModelState.IsValid) return false;
        JsonResult answer = userController.GetUser(new TokenVerification(requestAdmin.idAdmin, requestAdmin.token));
        User? user = answer.Value as User;
        if (user?.id == 0 || user?.admin == false) return false;
        else
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("UpdateAdmin", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", requestAdmin.idTarget);
                    con.Open();
                    cmd.ExecuteNonQuery();
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
                return false;
            }
        }
    }

    [HttpPost("updatemoney")]
    public bool UpdateMoney([FromBody] RequestAdminMoney requestAdminMoney)
    {
        if (!ModelState.IsValid) return false;
        JsonResult answer = userController.GetUser(new TokenVerification(requestAdminMoney.idAdmin, requestAdminMoney.token));
        User? user = answer.Value as User;
        if (user?.id == 0 || user?.admin == false) return false;
        else
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("UpdateMoneyNewSet", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", requestAdminMoney.idTarget);
                    cmd.Parameters.AddWithValue("@value", requestAdminMoney.money);
                    con.Open();
                    cmd.ExecuteNonQuery();
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
                return false;
            }
        }
    }
}