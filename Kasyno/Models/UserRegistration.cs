using System.ComponentModel.DataAnnotations;

namespace Models.UserModel;

public class UserRegistration
{
    [Display(Name = "login")]
    public string login { get; set; }
    [Display(Name = "email")]
    public string email { get; set; }
    [Display(Name = "password")]
    public string password { get; set; }
    [Display(Name = "c_password")]
    public string c_password { get; set; }
    public UserRegistration(string login, string email, string password, string c_password)
    {
        this.login = login;
        this.email = email;
        this.password = password;
        this.c_password = c_password;
    }
}