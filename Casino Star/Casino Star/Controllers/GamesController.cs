#pragma warning disable CS8601, CS8604, CS8618

using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Models.CoinFlip;
using Models.TokenVerification;
using Models.UserModel;
using Models.Roulette;
using Models.RouletteHistory;
using Models.CoinFlipHistory;

namespace Kasyno.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GamesController : ControllerBase
{
    private string connectionString;
    private static readonly HttpClient client = new HttpClient();
    private UserController userController;

    public GamesController(UserController userController, IConfiguration _configuration)
    {
        this.connectionString = _configuration.GetConnectionString("Database");
        this.userController = userController;
    }

    [HttpPost("coinflip")]
    public IActionResult CoinFlip([FromBody] CoinFlipRequest coinFlipRequest)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(new { statusCode = false, message = ModelState });
            if (coinFlipRequest.gameCounter != 10 && coinFlipRequest.gameCounter != 4 && coinFlipRequest.gameCounter != 2) return BadRequest(new { statusCode = false, message = "Something went wrong, please try later." });
            JsonResult answer = userController.GetUser(new TokenVerification(coinFlipRequest.id, coinFlipRequest.token));
            User? user = answer.Value as User;
            if (user?.money < coinFlipRequest.betMoney) return Unauthorized(new { statusCode = false, message = "You don't have enough funds to make this operation." });
            else
            {
                Random rnd = new Random();
                bool score = (rnd.NextDouble() <= (1.0 / coinFlipRequest.gameCounter));
                int winPrize;
                if (score) winPrize = coinFlipRequest.betMoney * coinFlipRequest.gameCounter;
                else winPrize = coinFlipRequest.betMoney * (-1);
                bool allowed = userController.ChargeAccountContinue(coinFlipRequest.id, winPrize);
                if (allowed)
                {
                    bool worked = false;
                    do
                    {
                        worked = !InsertCoinFlipHistory(coinFlipRequest.id, winPrize, (coinFlipRequest.decision ? "gold" : "silver"), coinFlipRequest.gameCounter);
                    } while (worked);
                    if (winPrize > 0) return Ok(new { statusCode = true, message = "You won " + winPrize + " dollars." });
                    else return Ok(new { statusCode = false, message = "You lost " + winPrize * (-1) + " dollars." });
                }
                else return UnprocessableEntity(new { statusCode = false, message = "Something went wrong, please try later." });
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

    [HttpPost("roulettenumber")]
    public IActionResult RouletteNumber([FromBody] RouletteNumberRequest rouletteNumberRequest)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(new { statusCode = false, message = ModelState, number = -1 });
            JsonResult answer = userController.GetUser(new TokenVerification(rouletteNumberRequest.id, rouletteNumberRequest.token));
            User? user = answer.Value as User;
            if (user?.money < rouletteNumberRequest.betMoney) return Unauthorized(new { statusCode = false, message = "You don't have enough funds to make this operation.", number = -1 });
            else
            {
                int score = GetRouletteNumber();
                int winPrize;
                if (score == rouletteNumberRequest.decision) winPrize = rouletteNumberRequest.betMoney * 10;
                else winPrize = rouletteNumberRequest.betMoney * (-1);
                bool allowed = userController.ChargeAccountContinue(rouletteNumberRequest.id, winPrize);
                if (allowed)
                {
                    bool worked = false;
                    do
                    {
                        worked = !InsertRoulleteHistory(rouletteNumberRequest.id, winPrize, rouletteNumberRequest.decision.ToString(), score.ToString());
                    } while (worked);
                    if (winPrize > 0) return Ok(new { statusCode = true, message = "You won " + winPrize + " dollars.", number = score });
                    else return Ok(new { statusCode = false, message = "You lost " + winPrize * (-1) + " dollars.", number = score });
                }
                else return UnprocessableEntity(new { statusCode = false, message = "Something went wrong, please try later.", number = -1 });
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.Message); // logger
            return UnprocessableEntity(new { statusCode = false, message = "Something went wrong, please try later.", number = -1 });
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message); // logger
            return UnprocessableEntity(new { statusCode = false, message = "Something went wrong, please try later.", number = -1 });
        }
    }

    [HttpPost("roulettecolor")]
    public IActionResult RouletteColor([FromBody] RouletteColorRequest rouletteColorRequest)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(new { statusCode = false, message = ModelState, number = -1 });
            JsonResult answer = userController.GetUser(new TokenVerification(rouletteColorRequest.id, rouletteColorRequest.token));
            User? user = answer.Value as User;
            if (user?.money < rouletteColorRequest.betMoney) return Unauthorized(new { statusCode = false, message = "You don't have enough funds to make this operation.", number = -1 });
            else
            {
                int score = GetRouletteNumber();
                string color = GetColorByField(score);
                int winPrize;
                if (color.Equals(rouletteColorRequest.decision)) winPrize = rouletteColorRequest.betMoney * ColorPrize(rouletteColorRequest.decision);
                else winPrize = rouletteColorRequest.betMoney * (-1);
                bool allowed = userController.ChargeAccountContinue(rouletteColorRequest.id, winPrize);
                if (allowed)
                {
                    bool worked = false;
                    do
                    {
                        worked = !InsertRoulleteHistory(rouletteColorRequest.id, winPrize, rouletteColorRequest.decision, color);
                    } while (worked);
                    if (winPrize > 0) return Ok(new { statusCode = true, message = "You won " + winPrize + " dollars.", number = score });
                    else return Ok(new { statusCode = false, message = "You lost " + winPrize * (-1) + " dollars.", number = score });

                }
                else return UnprocessableEntity(new { statusCode = false, message = "Something went wrong, please try later.", number = score });
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.Message); // logger
            return UnprocessableEntity(new { statusCode = false, message = "Something went wrong, please try later.", number = -1 });
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message); // logger
            return UnprocessableEntity(new { statusCode = false, message = "Something went wrong, please try later.", number = -1 });
        }
    }

    private int GetRouletteNumber()
    {
        Random rnd = new Random();
        return rnd.Next(1, 37);
    }

    private string GetColorByField(int number)
    {
        int[] tabOfRed = new int[] { 32, 19, 21, 25, 34, 27, 36, 30, 23, 5, 16, 1, 14, 9, 18, 7, 12, 3 };
        if (number == 0) return "green";
        else if (Array.Exists(tabOfRed, e => e == number)) return "red";
        else return "black";
    }

    private int ColorPrize(string color)
    {
        if (color.Equals("red")) return 2;
        else if (color.Equals("black")) return 2;
        else if (color.Equals("green")) return 10;
        else return 1;
    }

    private bool InsertRoulleteHistory(int id, int money, string decision, string decisionWin)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("InsertRouletteHistory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@money", money);
                cmd.Parameters.AddWithValue("@decision", decision);
                cmd.Parameters.AddWithValue("@decisionWin", decisionWin);
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
            throw;
        }
    }

    private bool InsertCoinFlipHistory(int id, int money, string decision, int decisionCounter)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("InsertCoinFlipHistory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@money", money);
                cmd.Parameters.AddWithValue("@decision", decision);
                cmd.Parameters.AddWithValue("@decisionCounter", decisionCounter);
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
            throw;
        }
    }

    [HttpGet("getroulettehistory")]
    public JsonResult GetRouletteHistory(int id, int count)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetRouletteHistory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@count", count);
                con.Open();
                SqlDataReader r = cmd.ExecuteReader();
                List<RouletteHistory> listOfHistory = new List<RouletteHistory>();
                while (r.Read())
                {
                    RouletteHistory rh = new RouletteHistory(
                        Convert.ToInt32(r["Id"]),
                        Convert.ToDateTime(r["Date"]),
                        Convert.ToInt32(r["WinMoney"]),
                        Convert.ToString(r["Decision"]),
                        Convert.ToString(r["WinDecision"]));
                    listOfHistory.Add(rh);
                }
                return new JsonResult(listOfHistory);
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.Message); // logger
            return new JsonResult(null);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message); // logger
            return new JsonResult(null);
        }
    }

    [HttpGet("getcoinfliphistory")]
    public JsonResult GetCoinFlipHistory(int id, int count)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetCoinFlipHistory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@count", count);
                con.Open();
                SqlDataReader r = cmd.ExecuteReader();
                List<CoinFlipHistory> listOfHistory = new List<CoinFlipHistory>();
                while (r.Read())
                {
                    CoinFlipHistory cfh = new CoinFlipHistory(
                        Convert.ToInt32(r["Id"]),
                        Convert.ToDateTime(r["Date"]),
                        Convert.ToInt32(r["WinMoney"]),
                        Convert.ToString(r["Decision"]),
                        Convert.ToString(r["DecisionCounter"]));
                    listOfHistory.Add(cfh);
                }
                return new JsonResult(listOfHistory);
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.Message); // logger
            return new JsonResult(null);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message); // logger
            return new JsonResult(null);
        }
    }
}