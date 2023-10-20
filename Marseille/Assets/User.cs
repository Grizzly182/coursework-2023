using Marseille.Assets;

namespace Marseille
{
    /// <summary>
    /// Статический класс, описывающий текущего пользователя,
    /// совершившего вход или регистрацию.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Уникальный ID пользователя из базы данных
        /// </summary>
        public uint Id { get; private set; }

        /// <summary>
        /// Логин пользователя.
        /// </summary>
        public string Login { get; private set; }

        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Фамилия пользователя.
        /// </summary>
        public string Surname { get; private set; }

        /// <summary>
        /// Отчество пользователя (если есть).
        /// </summary>
        public string Patronymic { get; private set; }

        /// <summary>
        /// Перечисление, служащее для определения прав пользователя в системе.
        /// </summary>
        public Role UserRole { get; private set; }

        public string RoleToString { get => UserRole.ToString(); }

        //TODO: Comment this
        /// <summary>
        ///
        /// </summary>
        public string ShortFullName { get => $"{Surname} {Name[0]}.{Patronymic[0]}."; } // Фамилия И.О.

        // TODO: Comment this later
        /// <summary>
        ///
        /// </summary>
        public static User CurrentUser { get; private set; }

        public User(uint id, string login, string name, string surname, string patronymic, Role userRole)
        {
            Id = id;
            Login = login;
            Name = name;
            Surname = surname;
            Patronymic = patronymic;
            UserRole = userRole;
        }

        /// <summary>
        /// Данный метод записывает в этот класс данные пользователя, вошедшего или
        /// зарегестрировавшегося в систему.
        /// </summary>
        /// <param name="login">Логин.</param>
        /// <param name="name">Имя.</param>
        /// <param name="surname">Фамилия.</param>
        /// <param name="patronymic">Отчество (если есть).</param>
        /// <param name="role">Права пользователя из перечисления.</param>
        public static void SetLoggedUser(uint id, string login, string name, string surname, string patronymic, Role role)
        {
            User currentUser = new User(id, login, name, surname, patronymic, role);
            CurrentUser = currentUser;
        }

        // TODO: Comment this later
        /// <summary>
        ///
        /// </summary>
        public static void LogOut()
        {
            CurrentUser = null;
            IsolatedStorageController.Clear();
        }
    }
}