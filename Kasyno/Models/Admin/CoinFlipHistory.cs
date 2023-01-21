using System.ComponentModel.DataAnnotations;

namespace Models.CoinFlipHistory;

public class CoinFlipHistory
{
    [Display(Name = "id")]
    public int id { get; set; }
    [Display(Name = "date")]
    public DateTime date { get; set; }
    [Display(Name = "winMoney")]
    public int winMoney { get; set; }
    [Display(Name = "decision")]
    public string decision { get; set; }
    [Display(Name = "decisionCounter")]
    public string decisionCounter { get; set; }
    public CoinFlipHistory(int id, DateTime date, int winMoney, string decision, string decisionCounter)
    {
        this.id = id;
        this.date = date;
        this.winMoney = winMoney;
        this.decision = decision;
        this.decisionCounter = decisionCounter;
    }
}