using System.ComponentModel.DataAnnotations;

namespace Models.TokenVerification;

public class TokenVerification
{
    [Required]
    [Display(Name = "id")]
    public int id { get; set; }
    [Required]
    [Display(Name = "token")]
    public string? token { get; set; }

    public TokenVerification(int id, string token)
    {
        this.id = id;
        this.token = token;
    }
}