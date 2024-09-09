using System.ComponentModel.DataAnnotations;

namespace Models.RequestAdmin;

public class RequestAdmin
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

    public RequestAdmin(int idAdmin, string token, int idTarget)
    {
        this.idAdmin = idAdmin;
        this.idTarget = idTarget;
        this.token = token;
    }
}