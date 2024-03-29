﻿using System;
using System.Text;

namespace HotelDB
{
    /// <summary>
    /// Простой статический класс. Служит только для преобразования
    /// строки в массив 8-ми разрядных чисел, используется в хэшировании
    /// пароля.
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// Вспомогательная функция, возвращающая строку.
        /// </summary>
        /// <param name="byteData">На вход идёт массив байтов.</param>
        /// <returns>Возвращает строковое представление массива байтов.</returns>
        public static string GetString(byte[] byteData)
        {
            return Convert.ToBase64String(byteData);
        }

        /// <summary>
        /// Вспомогательная функция, возвращающая массив байтов,
        /// который по сути является той же строкой только в другом представлении.
        /// </summary>
        /// <param name="str">Функция принимает строку в качестве единственного аргумента.</param>
        /// <returns>Возвращает массив байтов, конвертированных из строки</returns>
        public static byte[] GetBytes(string str)
        {
            return Encoding.ASCII.GetBytes(str);
        }
    }
}