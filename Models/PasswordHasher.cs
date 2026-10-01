using System.Security.Cryptography;
using System.Text;

namespace WebShopping.Models;

public static class PasswordHasher
{
    public static string Hash(string password)
    {
        byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToHexString(bytes); // ví dụ: "3A7BD3E2360A3D..."
    }
}