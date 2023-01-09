using System;

namespace Marseille
{
    /// <summary>
    ///
    /// </summary>
    internal class WrongLoginOrPasswordException : Exception
    {
        public override string Message => "Неверный логин или пароль";
    }
}