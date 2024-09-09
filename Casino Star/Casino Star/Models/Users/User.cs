using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Models.UserModel;

public class User
{
    [Display(Name = "id")]
    public int id { get; set; }
    [Display(Name = "login")]
    public string login { get; set; }
    [Display(Name = "email")]
    public string email { get; set; }
    [Display(Name = "admin")]
    public bool admin { get; set; }
    [Display(Name = "money")]
    public int money { get; set; }

    public User(int id, string login, string email, bool admin, int money)
    {
        this.id = id;
        this.login = login;
        this.email = email;
        this.admin = admin;
        this.money = money;
    }
}