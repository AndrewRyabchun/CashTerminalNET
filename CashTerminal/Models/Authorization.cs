using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using CashTerminal.Data;
using System.Security.Cryptography;

namespace CashTerminal.Models
{
    class Authorization
    {
        public Authorization(string username, string password)
        {
            IsValid = Authorize(username, password);
            if (IsValid)
                Username = username;
        }

        public bool IsValid { get; }

        public string Username { get; }

        //do async
        private bool Authorize(string username, string password)
        {
            var hash = SHA1HashStringForUTF8String(password).ToUpper();
            using (SupermarketDataEntities ent = new SupermarketDataEntities())
            {
                var wanted = from d in ent.Users
                             where d.Username == username &&
                                d.PasswordHash.ToUpper() == hash
                             select d;
                var res = wanted.ToList();
                return res.Count != 0;
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
