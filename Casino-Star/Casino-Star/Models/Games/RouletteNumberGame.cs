using System.ComponentModel.DataAnnotations;
using Models.BaseRequest;

namespace Models.Roulette;

public class RouletteNumberRequest : BaseRequest
{
    [Required]
    [Display(Name = "decision")]
    public int decision { get; set; }
    [Required]
    [Display(Name = "betMoney")]
    [Range(1, 1000000, ErrorMessage = "Bet money need to be between 1 and 1000000 dollars")]
    public int betMoney { get; set; }
}