using System.ComponentModel.DataAnnotations;

namespace Models.TokenVerification;

public class TokenVerificationWithEmailChange : TokenVerification
{
    [Required(ErrorMessage = "Old e-mail is required.")]
    [EmailAddress(ErrorMessage = "Old e-mail is incorrect.")]
    [Display(Name = "oldEmail")]
    public string? oldEmail { get; set; }
    [Required(ErrorMessage = "New e-mail is required.")]
    [EmailAddress(ErrorMessage = "New e-mail is incorrect.")]
    [Display(Name = "newEmail")]
    public string? newEmail { get; set; }

    public TokenVerificationWithEmailChange(int id, string token, string oldEmail, string newEmail) : base(id, token)
    {
        this.oldEmail = oldEmail;
        this.newEmail = newEmail;
    }
}