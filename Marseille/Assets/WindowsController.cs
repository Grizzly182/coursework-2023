using System.Windows;

namespace Marseille.Assets
{
    public static class WindowsController
    {
        public static void OpenWindow(Window window, Window parent)
        {
            window.Closed += (sender, e) => { parent.Show(); };
            parent.Hide();
            window.Show();
        }
    }
}