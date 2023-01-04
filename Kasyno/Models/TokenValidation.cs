using System.ComponentModel.DataAnnotations;

namespace Models.TokenVerification;

public class TokenVerification
{
    [Display(Name = "id")]
    public int id { get; set; }
    [Display(Name = "token")]
    public string? token { get; set; }
}