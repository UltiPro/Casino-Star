using System.ComponentModel.DataAnnotations;

namespace Models.TokenVerification;

public class TokenVerificationWithPasswordChange : TokenVerification
{
    [Required(ErrorMessage = "Old password is required.")]
    [RegularExpression(@"^(?=.*?[a-z])(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[.~!@#$%^&*()+=[\]\\;:'""/,|{}<>?])\S{8,40}$", ErrorMessage = "Incorrect expression of old password.")]
    [Display(Name = "oldPassword")]
    public string? oldPassword { get; set; }
    [Required(ErrorMessage = "New password is required.")]
    [StringLength(40, ErrorMessage = "New password should be 8 to 30 characters long.", MinimumLength = 8)]
    [RegularExpression(@"^(?=.*?[a-z])(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[.~!@#$%^&*()+=[\]\\;:'""/,|{}<>?])\S{8,40}$", ErrorMessage = "Incorrect expression of password.")]
    [Display(Name = "newPassword")]
    public string? newPassword { get; set; }
    [Required(ErrorMessage = "Confirm new password is required.")]
    [Compare("newPassword", ErrorMessage = "New passwords do not match!")]
    [Display(Name = "newPasswordRepeat")]
    public string? newPasswordRepeat { get; set; }
}