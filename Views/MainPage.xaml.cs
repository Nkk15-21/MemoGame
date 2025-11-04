using MemoGame.Models;
using MemoGame.Services;

namespace MemoGame.Views;

public partial class MainPage : ContentPage // Главная страница
{
    public MainPage() // конструктор
    {
        InitializeComponent();
        UpdateBest();
    }

    private void UpdateBest() // обновить лучший результат
    {
        var best = App.ScoreService.GetBest(); // получаем лучший результат из сервиса
        BestLabel.Text = best is null // обновляем текст
            ? "Пока нет результатов"
            : $"Лучший: {best.Player} — {best.Moves} ход(ов), {best.Time:mm\\:ss}"; // форматированный вывод
    }

    private async void StartClicked(object sender, EventArgs e) // обработчик кнопки "Начать игру"
    {
        var name = string.IsNullOrWhiteSpace(NameEntry.Text) ? "Player" : NameEntry.Text.Trim();
        var player = new Player { Name = name };

        // настройки можно расширять (выбор сложности), пока фиксировано 4x4
        var settings = new GameSettings { Rows = 4, Cols = 4 }; // 4x4 — 8 пар

        await Navigation.PushAsync(new GamePage(settings, player)); // переходим на страницу игры
    }

    private async void SettingsClicked(object sender, EventArgs e) // обработчик кнопки "Настройки"
    {
        await Navigation.PushAsync(new SettingsPage());
    }

    private async void LeaderboardClicked(object sender, EventArgs e)// обработчик кнопки "Таблица рекордов"
    {
        await Navigation.PushAsync(new LeaderboardPage());
    }

    protected override void OnAppearing()// при возврате на главную страницу
    {
        base.OnAppearing();
        UpdateBest(); // обновляем лучший результат при возврате на главную
    }
}
