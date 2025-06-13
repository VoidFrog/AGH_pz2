using System.Security.Cryptography;
using System.Text;

namespace Laboratorium09App.Helpers
{
    public static class HashHelper
    {
        public static string ObliczHash(string input)
        {
            using (var md5 = MD5.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(input);
                var hashBytes = md5.ComputeHash(bytes);
                return string.Concat(hashBytes.Select(b => b.ToString("x2")));
            }
        }
    }
}