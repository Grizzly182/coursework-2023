using System.Windows.Media;

namespace Marseille
{
    public enum ProjectColors
    {
        Main,
        Secondary,
        Background
    }

    public static class ColorChanger
    {
        public static SolidColorBrush ChangeColorTo(ProjectColors color)
        {
            switch (color)
            {
                case ProjectColors.Main:
                    return new SolidColorBrush(Color.FromArgb(87, 89, 234, 100));

                case ProjectColors.Secondary:
                    return new SolidColorBrush(Color.FromArgb(87, 89, 234, 100));

                case ProjectColors.Background:
                    return new SolidColorBrush(Colors.White);
            }
            return null;
        }
    }
}