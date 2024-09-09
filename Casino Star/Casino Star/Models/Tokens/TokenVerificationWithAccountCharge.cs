using System.ComponentModel.DataAnnotations;

namespace Models.TokenVerification;

public class TokenVerificationWithAccountCharge : TokenVerification
{
    [Required(ErrorMessage = "Charge ammount is required.")]
    [Display(Name = "value")]
    [Range(1, 1000000, ErrorMessage = "The charge must be greater than 1 and lower than 1000000.")]
    public int value { get; set; }

    public TokenVerificationWithAccountCharge(int id, string token, int value) : base(id, token)
    {
        this.value = value;
    }
}