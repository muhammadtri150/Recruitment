using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;


namespace FinalProject.Utils
{
    public class CryptographyUtils
    {
        public static string Encrypt(string str)
        {
            var algo = new SHA256Managed();
            var encode = algo.ComputeHash(Encoding.UTF8.GetBytes(str));
            var result = new StringBuilder();
            foreach (byte res in encode)
            {
                result.Append(res.ToString("x2"));
            }
            return result.ToString();
        }
    }
}