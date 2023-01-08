using System;

namespace HotelDB
{
    internal class WrongLoginOrPasswordException : Exception
    {
        public override string Message => "Неверный логин или пароль";
    }
}