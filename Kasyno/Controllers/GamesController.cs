#pragma warning disable CS8604

using Microsoft.AspNetCore.Mvc;
using Models.CoinFlip;
using Models.TokenVerification;
using Models.UserModel;
using Models.Roulette;

namespace Kasyno.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GamesController : ControllerBase
{
    private static readonly HttpClient client = new HttpClient();
    private UserController userController;
    public GamesController(UserController userController)
    {
        this.userController = userController;
    }
    [HttpPost("coinflip")]
    public IActionResult CoinFlip([FromBody] CoinFlipRequest coinFlipRequest)
    {
        if (!ModelState.IsValid) return Ok(new { statusCode = false, message = ModelState });
        if (coinFlipRequest.gameCounter != 10 && coinFlipRequest.gameCounter != 4 && coinFlipRequest.gameCounter != 2) return Ok(new { statusCode = false, message = "Something went wrong, please try later" });
        JsonResult answer = userController.GetUser(new TokenVerification(coinFlipRequest.id, coinFlipRequest.token));
        User? user = answer.Value as User;
        if (user?.money < coinFlipRequest.betMoney) return Ok(new { statusCode = false, message = "You don't have enough funds to make this operation" });
        else
        {
            Random rnd = new Random();
            bool score = (rnd.NextDouble() <= (1.0 / coinFlipRequest.gameCounter));
            if (score)
            {
                bool allowed = userController.ChargeAccountContinue(coinFlipRequest.id, coinFlipRequest.betMoney * coinFlipRequest.gameCounter);
                if (allowed) return Ok(new { statusCode = true, message = "You won " + coinFlipRequest.betMoney * coinFlipRequest.gameCounter + " dollars" });
                else return Ok(new { statusCode = false, message = "Something went wrong, please try later" });
            }
            else
            {
                bool allowed = userController.ChargeAccountContinue(coinFlipRequest.id, coinFlipRequest.betMoney * (-1));
                if (allowed) return Ok(new { statusCode = false, message = "You lost " + coinFlipRequest.betMoney + " dollars" });
                else return Ok(new { statusCode = false, message = "Something went wrong, please try later" });
            }
        }
    }
    [HttpPost("roulettenumber")]
    public IActionResult RouletteNumber([FromBody] RouletteNumberRequest rouletteNumberRequest)
    {
        if (!ModelState.IsValid) return Ok(new { statusCode = false, message = ModelState, number = -1 });
        JsonResult answer = userController.GetUser(new TokenVerification(rouletteNumberRequest.id, rouletteNumberRequest.token));
        User? user = answer.Value as User;
        if (user?.money < rouletteNumberRequest.betMoney) return Ok(new { statusCode = false, message = "You don't have enough funds to make this operation", number = -1 });
        else
        {
            int score = GetRouletteNumber();
            if (score == rouletteNumberRequest.decision)
            {
                int winPrize = rouletteNumberRequest.betMoney * 10;
                bool allowed = userController.ChargeAccountContinue(rouletteNumberRequest.id, winPrize);
                if (allowed) return Ok(new { statusCode = true, message = "You won " + winPrize + " dollars", number = score });
                else return Ok(new { statusCode = false, message = "Something went wrong, please try later", number = -1 });
            }
            else
            {
                bool allowed = userController.ChargeAccountContinue(rouletteNumberRequest.id, rouletteNumberRequest.betMoney * (-1));
                if (allowed) return Ok(new { statusCode = false, message = "You lost " + rouletteNumberRequest.betMoney + " dollars", number = score });
                else return Ok(new { statusCode = false, message = "Something went wrong, please try later", number = -1 });
            }
        }
    }
    [HttpPost("roulettecolor")]
    public IActionResult RouletteColor([FromBody] RouletteColorRequest rouletteColorRequest)
    {
        if (!ModelState.IsValid) return Ok(new { statusCode = false, message = ModelState, number = -1 });
        JsonResult answer = userController.GetUser(new TokenVerification(rouletteColorRequest.id, rouletteColorRequest.token));
        User? user = answer.Value as User;
        if (user?.money < rouletteColorRequest.betMoney) return Ok(new { statusCode = false, message = "You don't have enough funds to make this operation", number = -1 });
        else
        {
            int score = GetRouletteNumber();
            if (GetColorByField(score).Equals(rouletteColorRequest.decision))
            {
                int winPrize = rouletteColorRequest.betMoney * ColorPrize(rouletteColorRequest.decision);
                bool allowed = userController.ChargeAccountContinue(rouletteColorRequest.id, winPrize);
                if (allowed) return Ok(new { statusCode = true, message = "You won " + winPrize + " dollars", number = score });
                else return Ok(new { statusCode = false, message = "Something went wrong, please try later", number = score });
            }
            else
            {
                bool allowed = userController.ChargeAccountContinue(rouletteColorRequest.id, rouletteColorRequest.betMoney * (-1));
                if (allowed) return Ok(new { statusCode = false, message = "You lost " + rouletteColorRequest.betMoney + " dollars", number = score });
                else return Ok(new { statusCode = false, message = "Something went wrong, please try later", number = score });
            }
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
}