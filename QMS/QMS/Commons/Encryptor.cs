using System;
using System.Security.Cryptography;
using System.Text;

namespace QMS.Commons
{
    public static class Encryptor
    {
        public static string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits
                //for each byte
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }

        public static string Sha256_hash(string value)
        {
            byte[] passwordBytes = Encoding.Unicode.GetBytes(value);
            //
            // This is where you'd normally append or prepend the salt bytes
            //
            var hasher = HashAlgorithm.Create();
            byte[] hashedBytes = hasher.ComputeHash(passwordBytes);
            return Convert.ToBase64String(hashedBytes);
        }
    }
}
