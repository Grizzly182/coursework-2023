namespace Marseille.Assets
{
    public class Utility
    {
        /// <summary>
        /// Проверяет всю строку на наличие букв, отличающихся от английского алфавита.
        /// </summary>
        /// <param name="text">Строка передаваемая на проверку.</param>
        /// <returns>Возвращает <see langword="true"/> если в строке все буквы
        /// являются частью английского алфавита, иначе возвращает <see langword="false"/></returns>
        public static bool IsEnglishText(string text)
        {
            foreach (char ch in text.ToLower())
            {
                if (!(ch >= 97 && ch <= 122 || char.IsDigit(ch)))
                {
                    return false;
                }
            }

            return true;
        }
    }
}