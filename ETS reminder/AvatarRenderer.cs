using System.Windows.Controls;
using System.Windows.Media;
using Color = System.Windows.Media.Color;
using ColorConverter = System.Windows.Media.ColorConverter;
using FontFamily = System.Windows.Media.FontFamily;

namespace ETS_reminder;

public static class AvatarRenderer
{
    /// <summary>
    /// Updates a Border + TextBlock pair to show the current avatar (initials or symbol).
    /// </summary>
    public static void Render(Border border, TextBlock textBlock, UserProfile profile, double fontSize = 28)
    {
        border.Background = new SolidColorBrush(
            (Color)ColorConverter.ConvertFromString(profile.AvatarColor));

        if (profile.ActiveAvatarId == AvatarCatalog.InitialsId || string.IsNullOrEmpty(profile.ActiveAvatarId))
        {
            textBlock.Text = profile.Initials;
            textBlock.FontFamily = new FontFamily("Segoe UI");
            textBlock.FontSize = fontSize;
        }
        else
        {
            var avatar = AvatarCatalog.GetById(profile.ActiveAvatarId);
            if (avatar != null)
            {
                textBlock.Text = avatar.Symbol;
                textBlock.FontFamily = new FontFamily("Segoe UI Emoji");
                textBlock.FontSize = fontSize * 0.9;
            }
            else
            {
                textBlock.Text = profile.Initials;
                textBlock.FontFamily = new FontFamily("Segoe UI");
                textBlock.FontSize = fontSize;
            }
        }
    }

    /// <summary>
    /// Small avatar variant for menu bars and compact displays.
    /// </summary>
    public static void RenderSmall(Border border, TextBlock textBlock, UserProfile profile)
    {
        Render(border, textBlock, profile, fontSize: 10);
    }
}
