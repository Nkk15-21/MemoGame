using MemoGame.Services;
using MemoGame.Views;

namespace MemoGame;

public partial class App : Application
{
    public static ThemeManager ThemeManager { get; } = new ThemeManager(); // единый менеджер тем
    public static ScoreService ScoreService { get; } = new ScoreService();   // сервис рекордов (Preferences)

    public App()
    {
        InitializeComponent();

        // Применяем тему при старте (чтение сохранённого выбора пользователя)
        App.ThemeManager.ApplySavedOrDefaultTheme(this);

        // Навигация — стартуем с главной страницы
        MainPage = new NavigationPage(new MainPage());
    }
}
