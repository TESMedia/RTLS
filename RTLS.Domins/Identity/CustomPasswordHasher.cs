using Microsoft.AspNet.Identity;
using System.Security.Cryptography;

namespace RTLS.Domins.Identity
{
    public class CustomPasswordHasher 
    {
        public string HashPassword(string password)
        {
            return Encrypt.GetMD5Hash(password);
        }

        public PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            if (hashedPassword == HashPassword(providedPassword))
                return PasswordVerificationResult.Success;
            else
                return PasswordVerificationResult.Failed;
        }
        public class Encrypt
        {
            public static string GetMD5Hash(string input)
            {
                //Encrypt text using md5
                using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
                {
                    byte[] b = System.Text.Encoding.UTF8.GetBytes(input);
                    b = md5.ComputeHash(b);//Hash data
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    foreach (byte x in b)
                        sb.Append(x.ToString("x2"));//hexadecimal
                    return sb.ToString();
                }
            }
        }
    }
}
