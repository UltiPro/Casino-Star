using System.ComponentModel.DataAnnotations;

namespace Models.RouletteHistory;

public class RouletteHistory
{
    [Display(Name = "id")]
    public int id { get; set; }
    [Display(Name = "date")]
    public DateTime date { get; set; }
    [Display(Name = "winMoney")]
    public int winMoney { get; set; }
    [Display(Name = "decision")]
    public string decision { get; set; }
    [Display(Name = "decisionWin")]
    public string decisionWin { get; set; }
    public RouletteHistory(int id, DateTime date, int winMoney, string decision, string decisionWin)
    {
        this.id = id;
        this.date = date;
        this.winMoney = winMoney;
        this.decision = decision;
        this.decisionWin = decisionWin;
    }
}