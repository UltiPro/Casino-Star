using System.Text.RegularExpressions;

namespace Models.UserDataTests;

public class UserTests
{
    public static bool LoginTest(string login)
    {
        Regex rg = new Regex(@"^[A-Za-z][A-Za-z0-9_-]{1,13}[A-Za-z0-9]$");
        if (rg.IsMatch(login)) return true;
        return false;
    }
    public static bool EmailTest(string email)
    {
        Regex rg = new Regex(@"^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$");
        if (rg.IsMatch(email)) return true;
        return false;
    }
    public static bool PasswordTest(string password)
    {
        Regex rg = new Regex(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,30}$");
        if (rg.IsMatch(password)) return true;
        return false;
    }
    public static bool MatchPasswords(string password, string password2)
    {
        if (password.Equals(password2)) return true;
        return false;
    }
}