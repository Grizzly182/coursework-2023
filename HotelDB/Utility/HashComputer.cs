using System.Security.Cryptography;

namespace HotelDB
{
    /// <summary>
    /// Класс, служащий для вычисления хэша.
    /// </summary>
    public class HashComputer
    {
        /// <summary>
        /// Генерирует SHA256 хэш из строки (аргумента функции) с помощью
        /// функции ComputeHash(byte[] buffer).
        /// </summary>
        /// <param name="passwordWithSalt">Пароль с "насыпанной" солью</param>
        /// <returns>Возвращает строку хэша пароля </returns>
        /// <remarks>В качестве аргумента необходимо передавать строку пароля + строка соли</remarks>
        public string GetPasswordHashAndSalt(string passwordWithSalt)
        {
            SHA256 sha = new SHA256CryptoServiceProvider();
            byte[] dataBytes = Utility.GetBytes(passwordWithSalt);
            byte[] resultBytes = sha.ComputeHash(dataBytes);

            return Utility.GetString(resultBytes);
        }
    }
}