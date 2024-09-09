using System.ComponentModel.DataAnnotations;

namespace Models.UserModel;

public class UserRegistration
{
    [Required(ErrorMessage = "Login is required.")]
    [StringLength(30, ErrorMessage = "Login should be 3 to 30 characters long.", MinimumLength = 3)]
    [RegularExpression(@"^(?=.*?[a-zA-Z\d])[a-zA-Z][a-zA-Z\d_-]{2,28}[a-zA-Z\d]$", ErrorMessage = "Incorrect expression of login.")]
    [Display(Name = "login")]
    public string login { get; set; }
    [Required(ErrorMessage = "E-mail is required.")]
    [EmailAddress(ErrorMessage = "Incorrect address e-mail.")]
    [Display(Name = "email")]
    public string email { get; set; }
    [Required(ErrorMessage = "Password is required.")]
    [StringLength(40, ErrorMessage = "Password should be 8 to 40 characters long.", MinimumLength = 8)]
    [RegularExpression(@"^(?=.*?[a-z])(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[.~!@#$%^&*()+=[\]\\;:'""/,|{}<>?])\S{8,40}$", ErrorMessage = "Incorrect expression of password.")]
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