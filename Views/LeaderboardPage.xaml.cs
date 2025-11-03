using System.Globalization;
using MemoGame.Services;

namespace MemoGame.Views;

public partial class LeaderboardPage : ContentPage
{
    public LeaderboardPage()
    {
        InitializeComponent();

        // Простой конвертер "на лету" через ресурс страницы
        Resources.Add("ScoreTextConverter", new ScoreTextConverter());
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        var items = App.ScoreService.GetAll();
        ScoresView.ItemsSource = items;
        EmptyLabel.IsVisible = !items.Any();
    }
}

/// <summary>
/// Конвертер для красивого текста результата: "Ходы: X • Время: mm:ss"
/// </summary>
public class ScoreTextConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is ScoreEntry s)
            return $"Ходы: {s.Moves} • Время: {s.Time:mm\\:ss}";
        return "";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}
