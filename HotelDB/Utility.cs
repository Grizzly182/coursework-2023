using System;

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
        /// <param name="byteData">На вход идёт массив байтов(8-ми разрядных чисел).</param>
        /// <returns>Возвращает строковое представление массива 8-ми разрядных чисел.</returns>
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
            return Convert.FromBase64String(str);
        }
    }
}