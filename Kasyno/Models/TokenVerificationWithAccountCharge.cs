using System.ComponentModel.DataAnnotations;

namespace Models.TokenVerification;

public class TokenVerificationWithAccountCharge
{
    [Display(Name = "id")]
    public int id { get; set; }
    [Display(Name = "token")]
    public string? token { get; set; }
    [Required(ErrorMessage = "Charge ammount is required.")]
    [Display(Name = "value")]
    public int value { get; set; }
}