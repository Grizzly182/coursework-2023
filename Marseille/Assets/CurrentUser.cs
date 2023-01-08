namespace Marseille
{
    internal static class CurrentUser
    {
        public static string Login { get; private set; }
        public static string Name { get; private set; }
        public static string Surname { get; private set; }
        public static string Patronymic { get; private set; }
        public static Role UserRole { get; private set; }

        public static void SetLoggedUser(string login, string name, string surname, string patronymic, int role)
        {
            Login = login;
            Name = name;
            Surname = surname;
            Patronymic = patronymic;
            UserRole = (Role)role;
        }
    }
}