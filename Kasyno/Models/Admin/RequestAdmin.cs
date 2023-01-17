using System.ComponentModel.DataAnnotations;

namespace Models.RequestAdmin;

public class RequestAdmin
{
    [Required]
    [Display(Name = "idOfAdmin")]
    public int idAdmin { get; set; }
    [Required]
    [Display(Name = "token")]
    public string token { get; set; }
    [Required]
    [Display(Name = "idOfUser")]
    public int idUser { get; set; }
    public RequestAdmin(int idOfAdmin, string token, int idOfUser)
    {
        this.idAdmin = idOfAdmin;
        this.idUser = idOfUser;
        this.token = token;
    }
}