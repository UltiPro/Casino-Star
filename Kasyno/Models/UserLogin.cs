using System.ComponentModel.DataAnnotations;

namespace Models.UserModel;

public class UserLogin
{
    [Required(ErrorMessage = "Login is required.")]
    [StringLength(15, ErrorMessage = "Login should be 3 to 15 characters long.", MinimumLength = 3)]
    [RegularExpression(@"^[A-Za-z][A-Za-z0-9_-]{1,13}[A-Za-z0-9]$", ErrorMessage = "Incorrect expression of login.")]
    [Display(Name = "login")]
    public string login { get; set; }
    [Required(ErrorMessage = "Password is required.")]
    [StringLength(30, ErrorMessage = "Password should be 8 to 30 characters long.", MinimumLength = 8)]
    [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,30}$", ErrorMessage = "Incorrect expression of password.")]
    [Display(Name = "password")]
    public string password { get; set; }
    public UserLogin(string login, string password)
    {
        this.login = login;
        this.password = password;
    }
}