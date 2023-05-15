using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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