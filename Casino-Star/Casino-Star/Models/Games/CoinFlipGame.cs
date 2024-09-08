using System.ComponentModel.DataAnnotations;
using Models.BaseRequest;

namespace Models.CoinFlip;

public class CoinFlipRequest : BaseRequest
{
    [Required]
    [Display(Name = "decision")]
    public bool decision { get; set; }
    [Required]
    [Display(Name = "betMoney")]
    [Range(1, 1000000, ErrorMessage = "Bet money need to be between 1 and 1000000 dollars")]
    public int betMoney { get; set; }
    [Required]
    [Display(Name = "gameCounter")]
    public int gameCounter { get; set; }
}