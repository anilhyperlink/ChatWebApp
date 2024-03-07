using System.Security.Cryptography;
using System.Text;
using System;

namespace ChatWebApp.Models.Comman
{
    public class AppEncrypt
    {
        public static string CreateHash(string password)
        {
            var provider = MD5.Create();
            string salt = "ghr3@7dby$8hb";
            byte[] bytes = provider.ComputeHash(Encoding.UTF32.GetBytes(salt + password));
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }
    }
}
