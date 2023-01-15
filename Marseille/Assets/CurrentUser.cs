namespace Marseille
{
    /// <summary>
    /// Статический класс, описывающий текущего пользователя,
    /// совершившего вход или регистрацию.
    /// </summary>
    public static class CurrentUser
    {
        /// <summary>
        /// Логин пользователя.
        /// </summary>
        public static string Login { get; private set; }

        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public static string Name { get; private set; }

        /// <summary>
        /// Фамилия пользователя.
        /// </summary>
        public static string Surname { get; private set; }

        /// <summary>
        /// Отчество пользователя (если есть).
        /// </summary>
        public static string Patronymic { get; private set; }

        /// <summary>
        /// Перечисление, служащее для определения прав пользователя в системе.
        /// </summary>
        public static Role UserRole { get; private set; }

        /// <summary>
        /// Данный метод записывает в этот класс данные пользователя, вошедшего или
        /// зарегестрировавшегося в систему.
        /// </summary>
        /// <param name="login">Логин.</param>
        /// <param name="name">Имя.</param>
        /// <param name="surname">Фамилия.</param>
        /// <param name="patronymic">Отчество (если есть).</param>
        /// <param name="role">Права пользователя из перечисления.</param>
        public static void SetLoggedUser(string login, string name, string surname, string patronymic, Role role)
        {
            Login = login;
            Name = name;
            Surname = surname;
            Patronymic = patronymic;
            UserRole = role;
        }
    }
}