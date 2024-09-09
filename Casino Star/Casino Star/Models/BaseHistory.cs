using System.ComponentModel.DataAnnotations;

namespace Models.BaseHistory;

public class BaseHistory
{
    [Display(Name = "id")]
    public int id { get; set; }
    [Display(Name = "date")]
    public DateTime date { get; set; }
    [Display(Name = "winMoney")]
    public int winMoney { get; set; }
    [Display(Name = "decision")]
    public string decision { get; set; }

    public BaseHistory(int id, DateTime date, int winMoney, string decision)
    {
        this.id = id;
        this.date = date;
        this.winMoney = winMoney;
        this.decision = decision;
    }
}