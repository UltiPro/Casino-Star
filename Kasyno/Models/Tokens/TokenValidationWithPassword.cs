using System.ComponentModel.DataAnnotations;

namespace Models.TokenVerification;

public class TokenVerificationWithPassword
{
    [Required]
    [Display(Name = "id")]
    public int id { get; set; }
    [Required]
    [Display(Name = "token")]
    public string? token { get; set; }
    [Required(ErrorMessage = "Password is required.")]
    [StringLength(30, ErrorMessage = "Password should be 8 to 30 characters long.", MinimumLength = 8)]
    [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,30}$", ErrorMessage = "Incorrect expression of password.")]
    [Display(Name = "password")]
    public string? password { get; set; }
}