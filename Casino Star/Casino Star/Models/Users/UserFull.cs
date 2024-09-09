using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Models.UserModel;

public class UserFull : User
{
    [Display(Name = "active")]
    public bool active { get; set; }
    [Display(Name = "banned")]
    public bool banned { get; set; }

    public UserFull(int id, string login, string email, bool admin, int money, bool active, bool banned) : base(id, login, email, admin, money)
    {
        this.active = active;
        this.banned = banned;
    }
}