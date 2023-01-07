using System.ComponentModel.DataAnnotations;

namespace Models.TokenVerification;

public class TokenVerificationWithEmailChange
{
    [Display(Name = "id")]
    public int id { get; set; }
    [Display(Name = "token")]
    public string? token { get; set; }
    [Required(ErrorMessage = "Old e-mail is required.")]
    [EmailAddress(ErrorMessage = "Old e-mail is incorrect.")]
    [RegularExpression(@"^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$", ErrorMessage = "Incorrect expression of old e-mail.")]
    [Display(Name = "oldEmail")]
    public string? oldEmail { get; set; }
    [Required(ErrorMessage = "New e-mail is required.")]
    [EmailAddress(ErrorMessage = "New e-mail is incorrect.")]
    [RegularExpression(@"^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$", ErrorMessage = "Incorrect expression of new e-mail.")]
    [Display(Name = "newEmail")]
    public string? newEmail { get; set; }
}