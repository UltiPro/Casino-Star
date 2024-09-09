using System.ComponentModel.DataAnnotations;

namespace Models.CoinFlipHistory;

public class CoinFlipHistory : BaseHistory.BaseHistory
{
    [Display(Name = "decisionCounter")]
    public string decisionCounter { get; set; }

    public CoinFlipHistory(int id, DateTime date, int winMoney, string decision, string decisionCounter) : base(id, date, winMoney, decision)
    {
        this.decisionCounter = decisionCounter;
    }
}