using System.ComponentModel.DataAnnotations;

namespace Models.RequestAdminMoney;

public class RequestAdminMoney : RequestAdmin.RequestAdmin
{
    [Required]
    [Display(Name = "money")]
    public int money { get; set; }

    public RequestAdminMoney(int idAdmin, string token, int idTarget, int money) : base(idAdmin, token, idTarget)
    {
        this.money = money;
    }
}