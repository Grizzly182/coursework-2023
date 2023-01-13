namespace HotelDB
{
    /// <summary>
    /// Выполняет все функции, связанные с изменением,
    /// созданием и хэшированием паролей.
    /// </summary>
    public class PasswordManager
    {
        /// <summary>
        /// Экземпляр класса <see cref="HashComputer"/>, для вычисления хэша.
        /// </summary>
        private readonly HashComputer hashComputer = new HashComputer();

        /// <summary>
        /// Эта функция генерирует хэш пароля и соль.
        /// </summary>
        /// <param name="password">Пароль передаётся в аргументах, из него генерируется хэш.</param>
        /// <param name="salt">Соль генерируется отдельно и присваивается в переменную salt, созданную за пределами функции.</param>
        /// <returns>Возвращает сгенерированный SHA256 хэш пароля.</returns>
        public string GeneratePasswordHash(string password, out string salt)
        {
            salt = SaltGenerator.GenerateSaltString();
            string finalString = password + salt;
            return hashComputer.GetPasswordHashAndSalt(finalString);
        }

        /// <summary>
        /// Проверяет, одинаковы ли пароли, сравнивая их хэш.
        /// </summary>
        /// <param name="password">Нехэшированный пароль.</param>
        /// <param name="salt">Соль, сгенерированная классом <see href="SaltGenerator"/>.</param>
        /// <param name="secondPasswordHash">Хэш пароля, с которым сравниваем с уже "насыпанной" солью.</param>
        /// <returns>Возвращает <see langword="true"/>, если хэши паролей равны, иначе - <see langword="false"/>.</returns>
        public bool IsPasswordsMatch(string password, string salt, string secondPasswordHash)
        {
            string firstPassword = password + salt;
            return secondPasswordHash == hashComputer.GetPasswordHashAndSalt(firstPassword);
        }
    }
}