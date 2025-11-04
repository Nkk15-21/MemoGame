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
                r["BgColor"] = Color.FromArgb("#FAFAFA");// светлый фон
                r["CardColor"] = Color.FromArgb("#FFFFFF");// белые карточки
                r["AccentColor"] = Color.FromArgb("#1976d2");// синий акцент
                r["TextColor"] = Color.FromArgb("#111111");// тёмный текст
                break;

            case AppThemeName.Dark:
                r["BgColor"] = Color.FromArgb("#121212");// тёмный фон
                r["CardColor"] = Color.FromArgb("#1f1f1f");// тёмные карточки
                r["AccentColor"] = Color.FromArgb("#4caf50");// зелёный акцент
                r["TextColor"] = Color.FromArgb("#ffffff");// светлый текст
                break;

            case AppThemeName.Colorful:
                r["BgColor"] = Color.FromArgb("#0e0a1f");// тёмный фиолетовый фон
                r["CardColor"] = Color.FromArgb("#2a1b3d");// фиолетовые карточки
                r["AccentColor"] = Color.FromArgb("#ff6f00");// оранжевый акцент
                r["TextColor"] = Color.FromArgb("#ffecb3");// светлый текст с жёлтым оттенком
                break;
        }
    }
}
