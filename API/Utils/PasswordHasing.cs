namespace API.Utils
{
    public class PasswordHasing
    {
        public static string GetHashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);  
        }
    }
}
