using System.ComponentModel.DataAnnotations;

namespace Models.UserModel;

public class UserLogin
{
    [Display(Name = "login")]
    public string login { get; set; }
    [Display(Name = "password")]
    public string password { get; set; }
    public UserLogin(string login, string password)
    {
        this.login = login;
        this.password = password;
    }
}