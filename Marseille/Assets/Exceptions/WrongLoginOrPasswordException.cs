﻿using System;

namespace Marseille.Assets.Exceptions
{
    internal class WrongLoginOrPasswordException : Exception
    {
        public override string Message => "Wrong login or password entered!";
    }
}