using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;

namespace BotMood
{

    //Hash
    //var hash = SecurePasswordHasher.Hash("mypassword");
    //Verify
    //var result = SecurePasswordHasher.Verify("mypassword", hash);

    public class CryptoService
    {
        public static string Hash(string text)
        {
            return Hash(text, "$2017");
        }

        private static string Hash(string password, string saltText)
        {
            byte[] plainText = Encoding.UTF8.GetBytes(password);
            byte[] salt = Encoding.UTF8.GetBytes(saltText);

            HashAlgorithm algorithm = new SHA256Managed();

            byte[] plainTextWithSaltBytes =
              new byte[plainText.Length + salt.Length];

            for (int i = 0; i < plainText.Length; i++)
            {
                plainTextWithSaltBytes[i] = plainText[i];
            }
            for (int i = 0; i < salt.Length; i++)
            {
                plainTextWithSaltBytes[plainText.Length + i] = salt[i];
            }

            var hashArray = algorithm.ComputeHash(plainTextWithSaltBytes);
            var hashText = Convert.ToBase64String(hashArray);
            return hashText;
        }

        public static bool Verify(string password, string saltText, string hashedPassword)
        {
            var generatedHash = Hash(password, saltText);
            return generatedHash == hashedPassword;
        }


        public static string GenerateRandomCode()
        {
            var rnd = new Random(DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + DateTime.Now.Millisecond);
            var aux = rnd.Next(999999);
            return aux.ToString();
        }


    }

}
