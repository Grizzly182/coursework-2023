namespace HotelDB
{
    /// <summary>
    /// Выполняет все функции, связанные с изменением,
    /// созданием и хэшированием паролей.
    /// </summary>
    public class PasswordManager
    {
        private readonly HashComputer hashComputer = new HashComputer();

        public string GeneratePasswordHash(string password, out string salt)
        {
            salt = SaltGenerator.GenerateSaltString();
            string finalString = password + salt;
            return hashComputer.GetPasswordHashAndSalt(finalString);
        }

        public bool IsPasswordsMatch(string password, string salt, string secondPasswordHash)
        {
            string firstPassword = password + salt;
            return secondPasswordHash == hashComputer.GetPasswordHashAndSalt(firstPassword);
        }
    }
}