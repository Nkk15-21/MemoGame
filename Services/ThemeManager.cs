using Microsoft.Maui.Storage;

namespace MemoGame.Services;


// Менеджер тем. Подменяет динамические ресурсы Application.

public enum AppThemeName { Light, Dark, Colorful } // возможные темы

public class ThemeManager // менеджер тем
{
    private const string PrefKey = "app_theme";

    public AppThemeName CurrentTheme { get; private set; } = AppThemeName.Dark;

    public void ApplySavedOrDefaultTheme(Application app) // применить сохранённую или тему по умолчанию
    {
        // читаем сохранённую тему
        var saved = Preferences.Get(PrefKey, nameof(AppThemeName.Dark));
        if (Enum.TryParse<AppThemeName>(saved, out var t))
            ApplyTheme(app, t);
        else
            ApplyTheme(app, AppThemeName.Dark);
    }

    public void ApplyTheme(Application app, AppThemeName theme) // применить тему
    {
        CurrentTheme = theme;
        Preferences.Set(PrefKey, theme.ToString());

        // Получаем словарь ресурсов приложения
        var r = app.Resources;

        // В зависимости от выбранной темы меняем набор цветов
        switch (theme)
        {
            case AppThemeName.Light:
                r["BgColor"] = Color.FromArgb("#FAFAFA");
                r["CardColor"] = Color.FromArgb("#FFFFFF");
                r["AccentColor"] = Color.FromArgb("#1976d2");
                r["TextColor"] = Color.FromArgb("#111111");
                break;

            case AppThemeName.Dark:
                r["BgColor"] = Color.FromArgb("#121212");
                r["CardColor"] = Color.FromArgb("#1f1f1f");
                r["AccentColor"] = Color.FromArgb("#4caf50");
                r["TextColor"] = Color.FromArgb("#ffffff");
                break;

            case AppThemeName.Colorful:
                r["BgColor"] = Color.FromArgb("#0e0a1f");
                r["CardColor"] = Color.FromArgb("#2a1b3d");
                r["AccentColor"] = Color.FromArgb("#ff6f00");
                r["TextColor"] = Color.FromArgb("#ffecb3");
                break;
        }
    }
}
