using System.Security.Cryptography;

namespace HotelDB
{
    /// <summary>
    ///
    /// </summary>
    public class HashComputer
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="passwordWithSalt">Пароль с "насыпанной" солью</param>
        /// <returns></returns>
        public string GetPasswordHashAndSalt(string passwordWithSalt)
        {
            SHA256 sha = new SHA256CryptoServiceProvider();
            byte[] dataBytes = Utility.GetBytes(passwordWithSalt);
            byte[] resultBytes = sha.ComputeHash(dataBytes);

            return Utility.GetString(resultBytes);
        }
    }
}