using System.ComponentModel.DataAnnotations;

namespace Models.RouletteHistory;

public class RouletteHistory : BaseHistory.BaseHistory
{
    [Display(Name = "decisionWin")]
    public string decisionWin { get; set; }

    public RouletteHistory(int id, DateTime date, int winMoney, string decision, string decisionWin) : base(id, date, winMoney, decision)
    {
        this.decisionWin = decisionWin;
    }
}