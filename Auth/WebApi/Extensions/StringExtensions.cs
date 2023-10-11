using System.Security.Cryptography;
using System.Text;

namespace WebApi.Extensions
{
    public static class StringExtensions
    {
        public static string CreateMD5Hash(this string token)
        {
            using MD5 md5Hash = MD5.Create();
            string hash = GetMd5Hash(md5Hash, token);

            return hash;
        }

        //https://stackoverflow.com/questions/57310305/use-md5-to-encrypt-the-password-in-asp-net-core - code from here
        static string GetMd5Hash(MD5 md5Hash, string input)
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
    }
}
