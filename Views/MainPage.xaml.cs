using MemoGame.Models;
using MemoGame.Services;

namespace MemoGame.Views;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        UpdateBest();
    }

    private void UpdateBest()
    {
        var best = App.ScoreService.GetBest();
        BestLabel.Text = best is null
            ? "Пока нет результатов"
            : $"Лучший: {best.Player} — {best.Moves} ход(ов), {best.Time:mm\\:ss}";
    }

    private async void StartClicked(object sender, EventArgs e)
    {
        var name = string.IsNullOrWhiteSpace(NameEntry.Text) ? "Player" : NameEntry.Text.Trim();
        var player = new Player { Name = name };

        // настройки можно расширять (выбор сложности), пока фиксировано 4x4
        var settings = new GameSettings { Rows = 4, Cols = 4 };

        await Navigation.PushAsync(new GamePage(settings, player));
    }

    private async void SettingsClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SettingsPage());
    }

    private async void LeaderboardClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LeaderboardPage());
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        UpdateBest(); // обновляем лучший результат при возврате на главную
    }
}
