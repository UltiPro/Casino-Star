using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Models.CoinFlip;
using Models.PostAnswer;

namespace Kasyno.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GamesController : ControllerBase
{
    private static readonly HttpClient client = new HttpClient();
    public GamesController() { }

    [HttpGet("coinflip")]
    public IActionResult CoinFlip([FromBody] CoinFlipRequest coinFlipRequest)
    {
        if (!ModelState.IsValid) return BadRequest(new { statusCode = false, message = ModelState });
        if(coinFlipRequest.gameCounter != 5 && coinFlipRequest.gameCounter != 2 && coinFlipRequest.gameCounter != 1) return BadRequest(new { statusCode = false, message = "Something went wrong, please try later" });
        var answer = client.PostAsJsonAsync("http://localhost:5077", new { id = coinFlipRequest.id, token = coinFlipRequest.token, value = coinFlipRequest.betMoney });       
        

        Random rnd = new Random();
        //return Ok((int)Math.Round(rnd.NextDouble()));
        return Ok(answer.ToString());
    }
}