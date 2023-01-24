using System.ComponentModel.DataAnnotations;

namespace Models.RequestAdminMoney;

public class RequestAdminMoney
{
    [Required]
    [Display(Name = "idAdmin")]
    public int idAdmin { get; set; }
    [Required]
    [Display(Name = "token")]
    public string token { get; set; }
    [Required]
    [Display(Name = "idUser")]
    public int idTarget { get; set; }
    [Required]
    [Display(Name = "money")]
    public int money { get; set; }
    public RequestAdminMoney(int idAdmin, string token, int idTarget, int money)
    {
        this.idAdmin = idAdmin;
        this.idTarget = idTarget;
        this.token = token;
        this.money = money;
    }
}