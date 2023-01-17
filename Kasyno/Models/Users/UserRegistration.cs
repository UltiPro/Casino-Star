using System.ComponentModel.DataAnnotations;

namespace Models.UserModel;

public class UserRegistration
{
    [Required(ErrorMessage = "Login is required.")]
    [StringLength(15, ErrorMessage = "Login should be 3 to 15 characters long.", MinimumLength = 3)]
    [RegularExpression(@"^[A-Za-z][A-Za-z0-9_-]{1,13}[A-Za-z0-9]$", ErrorMessage = "Incorrect expression of login.")]
    [Display(Name = "login")]
    public string login { get; set; }
    [Required(ErrorMessage = "E-mail is required.")]
    [EmailAddress(ErrorMessage = "Incorrect address e-mail.")]
    [RegularExpression(@"^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$", ErrorMessage = "Incorrect expression of e-mail.")]
    [Display(Name = "email")]
    public string email { get; set; }
    [Required(ErrorMessage = "Password is required.")]
    [StringLength(30, ErrorMessage = "Password should be 8 to 30 characters long.", MinimumLength = 8)]
    [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,30}$", ErrorMessage = "Incorrect expression of password.")]
    [Display(Name = "password")]
    public string password { get; set; }
    [Required(ErrorMessage = "Confirm password is required.")]
    [Compare("password", ErrorMessage = "The passwords do not match!")]
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