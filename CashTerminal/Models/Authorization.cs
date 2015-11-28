using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using CashTerminal.Data;
using System.Security.Cryptography;

namespace CashTerminal.Models
{
    /// <summary>
    /// Отвечает за авторизацию пользователя
    /// </summary>
    internal class Authorization
    {
        /// <summary>
        /// Инициализирует экземпляр класса Authorization, используя заданные имя пользователя и пароль.
        /// </summary>
        /// <param name="username">Имя пользователя</param>
        /// <param name="password">Пароль пользоваетля</param>
        public Authorization(string username, string password)
        {
            IsValid = Authorize(username, password);
            if (IsValid)
                Username = username;
        }

        /// <summary>
        /// Проверяет пройденность пользователем процедуры авторизации.
        /// </summary>
        public bool IsValid { get; }

        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string Username { get; }

        /// <summary>
        /// Обеспечивает авторизацию.
        /// </summary>
        /// <param name="username">Имя пользователя.</param>
        /// <param name="password">Пароль пользователя.</param>
        /// <returns>Результат авторизации.</returns>
        private bool Authorize(string username, string password)
        {
            var hash = SHA1HashStringForUTF8String(password).ToUpper();
            using (SupermarketDataEntities ent = new SupermarketDataEntities())
            {
                var wanted = from d in ent.Users
                             where d.Username == username &&
                                d.PasswordHash.ToUpper() == hash
                             select d;
                try
                {
                    var res = wanted.ToList();
                    return res.Count != 0;
                }
                catch
                {
                    return false;
                }
                
            }
        }

        /// <summary>
        /// Производит хеширование строки по алгоритму SHA1.
        /// </summary>
        /// <param name="s">Строка для шифрования.</param>
        /// <returns>Строка, зашифрованая по алгоритму SHA1.</returns>
        private string SHA1HashStringForUTF8String(string s)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(s);

            using (var sha1 = SHA1.Create())
            {
                byte[] hashBytes = sha1.ComputeHash(bytes);

                return HexStringFromBytes(hashBytes);
            }
        }

        /// <summary>
        /// Переводит масив байтов в строку цифр шестнадцатеричной системы счисления.
        /// </summary>
        /// <param name="bytes">Массив для преобразования.</param>
        /// <returns>Строка цифр шестнадцатеричной системы счисления.</returns>
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
