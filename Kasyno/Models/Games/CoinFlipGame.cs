using System.ComponentModel.DataAnnotations;

namespace Models.CoinFlip;

public class CoinFlipRequest
{
    [Required]
    [Display(Name = "id")]
    public int id { get; set; }
    [Required]
    [Display(Name = "token")]
    public string? token { get; set; }
    [Required]
    [Display(Name = "decision")]
    public bool decision { get; set; }
    [Required]
    [Display(Name = "betMoney")]
    public int betMoney { get; set; }
    [Required]
    [Display(Name = "gameCounter")]
    public int gameCounter { get; set; }
}