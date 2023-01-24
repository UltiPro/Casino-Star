using System.ComponentModel.DataAnnotations;

namespace Models.TokenVerification;

public class TokenVerificationWithPasswordChange
{
    [Required]
    [Display(Name = "id")]
    public int id { get; set; }
    [Required]
    [Display(Name = "token")]
    public string? token { get; set; }
    [Required(ErrorMessage = "Old password is required.")]
    [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,30}$", ErrorMessage = "Incorrect expression of old password.")]
    [Display(Name = "oldPassword")]
    public string? oldPassword { get; set; }
    [Required(ErrorMessage = "New password is required.")]
    [StringLength(30, ErrorMessage = "New password should be 8 to 30 characters long.", MinimumLength = 8)]
    [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,30}$", ErrorMessage = "Incorrect expression of password.")]
    [Display(Name = "newPassword")]
    public string? newPassword { get; set; }
    [Required(ErrorMessage = "Confirm new password is required.")]
    [Compare("newPassword", ErrorMessage = "New passwords do not match!")]
    [Display(Name = "newPasswordRepeat")]
    public string? newPasswordRepeat { get; set; }
}