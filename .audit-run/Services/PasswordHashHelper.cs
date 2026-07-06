using System.Security.Cryptography;
using System.Text;

namespace PerfumeStore.MVCC.Services;

public static class PasswordHashHelper
{
    public static string ComputeSha256(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));

        return Convert.ToHexString(bytes);
    }
}
