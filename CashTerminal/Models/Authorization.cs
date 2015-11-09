using System;
using System.Linq;
using System.Text;
using CashTerminal.Data;
using System.Security.Cryptography;

namespace CashTerminal.Models
{
    class Authorization
    {
        public readonly bool validated;

        public Authorization(string username, string password)
        {
            validated = Authorize(username, password);
        }

        private bool Authorize(string username, string password)
        {
            using (SupermarketDataEntities ent = new SupermarketDataEntities())
            {
                var wanted = from d in ent.Users
                             where d.Username == username && d.PasswordHash == SHA1HashStringForUTF8String(password)
                             select d;

                return wanted != null;
            }
        }

        private string SHA1HashStringForUTF8String(string s)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(s);

            using (var sha1 = SHA1.Create())
            {
                byte[] hashBytes = sha1.ComputeHash(bytes);

                return HexStringFromBytes(hashBytes);
            }
        }

        private static string HexStringFromBytes(byte[] bytes)
        {
            var sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                var hex = b.ToString("x2");
                sb.Append(hex);
            }
            return sb.ToString();
        }
    }
}
