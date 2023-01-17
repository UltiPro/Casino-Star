using System.ComponentModel.DataAnnotations;

namespace Models.TokenVerification;

public class TokenVerificationWithAccountCharge
{
    [Required(ErrorMessage = "Id is required.")]
    [Display(Name = "id")]
    public int id { get; set; }
    [Required(ErrorMessage = "Token is required.")]
    [Display(Name = "token")]
    public string? token { get; set; }
    [Required(ErrorMessage = "Charge ammount is required.")]
    [Display(Name = "value")]
    [Range(1, Int32.MaxValue, ErrorMessage = "The charge must be greater than 1.")]
    public int value { get; set; }
}