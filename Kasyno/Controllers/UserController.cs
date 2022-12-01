using Microsoft.AspNetCore.Mvc;
using Models.UserModel;

namespace Kasyno.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    public UserController() {

    }

    [HttpGet]
    public JsonResult Get()
    {
        return new JsonResult(new User(0, "Ulti", "ultiprogames@gmail.com", true, 1525));
    }
}