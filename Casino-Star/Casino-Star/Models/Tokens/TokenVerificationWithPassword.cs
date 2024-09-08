using System.ComponentModel.DataAnnotations;

namespace Models.TokenVerification;

public class TokenVerificationWithPassword : TokenVerification
{
    [Required(ErrorMessage = "Password is required.")]
    [StringLength(40, ErrorMessage = "Password should be 8 to 40 characters long.", MinimumLength = 8)]
    [RegularExpression(@"^(?=.*?[a-z])(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[.~!@#$%^&*()+=[\]\\;:'""/,|{}<>?])\S{8,40}$", ErrorMessage = "Incorrect expression of password.")]
    [Display(Name = "password")]
    public string? password { get; set; }
}