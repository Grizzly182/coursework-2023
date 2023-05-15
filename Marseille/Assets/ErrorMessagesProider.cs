using System.Windows;

namespace Marseille.Assets
{
    //TODO: Comment this
    /// <summary>
    /// Вспомогательный класс для быстрой обработки ошибок
    /// </summary>
    internal static class ErrorMessagesProider
    {
        public static void ShowError(string message)
        {
            MessageBox.Show(message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static MessageBoxResult ShowWarning(string message)
        {
            return MessageBox.Show(message, "Предупреждение.", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
        }
    }
}