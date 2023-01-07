using System.Security.Cryptography;

namespace HotelDB
{
    /// <summary>
    /// Класс, генерирующий "соль" для пароля пользователя.
    /// </summary>
    public static class SaltGenerator
    {
        /// <summary>
        /// Поле в котором может содержаться криптографический генератор случайных чисел (RNGCryptoServiceProvider)
        /// </summary>
        private static RNGCryptoServiceProvider cryptoServiceProvider = null;

        /// <summary>
        /// Размер массива байтов
        /// </summary>
        /// Значение по умолчанию:
        /// <value>12</value>
        private const int SALT_SIZE = 12; //при значении 12 итоговая длина хэша равна 16

        /// <summary>
        /// Конструктор, инициализирующий объект класса RNGCryptoServiceProvider
        /// </summary>
        /// <code><c>cryptoServiceProvider = new RNGCryptoServiceProvider();</c></code>
        static SaltGenerator()
        {
            cryptoServiceProvider = new RNGCryptoServiceProvider();
        }

        /// <summary>
        /// Функция генерации случайной "соли".
        /// </summary>
        ///
        /// <returns>Строку случайно сгенерированного хэша.</returns>
        /// <remarks>Содержание функции:</remarks>
        /// <code>
        /// //Создаём массив байтов
        /// byte[] saltBytes = new byte[SALT_SIZE];
        ///
        /// //Генерируем в массив случайные не нулевые значения
        /// cryptoServiceProvider.GetNonZeroBytes(saltBytes);
        ///
        /// //Возвращаем строку получившегося хэша заданного размера
        /// return Utility.GetString(saltBytes);</code>
        public static string GenerateSaltString()
        {
            //Создаём массив байтов
            byte[] saltBytes = new byte[SALT_SIZE];

            //Генерируем в массив случайные не нулевые значения
            cryptoServiceProvider.GetNonZeroBytes(saltBytes);

            //Воозвращаем строку получившегося хэша заданного размера
            return Utility.GetString(saltBytes);
        }
    }
}