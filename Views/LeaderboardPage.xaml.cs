using System.Globalization;
using MemoGame.Services;

namespace MemoGame.Views;

public partial class LeaderboardPage : ContentPage // Страница с таблицей рекордов
{
    public LeaderboardPage()
    {
        InitializeComponent();

        // Простой конвертер "на лету" через ресурс страницы
        Resources.Add("ScoreTextConverter", new ScoreTextConverter());
    }

    protected override void OnAppearing() // При появлении страницы загружаем результаты
    {
        base.OnAppearing(); // вызываем базовый метод
        var items = App.ScoreService.GetAll(); // получаем все результаты из сервиса
        ScoresView.ItemsSource = items; // привязываем к ListView
        EmptyLabel.IsVisible = !items.Any(); // показываем надпись, если нет результатов
    }
}

//Конвертер для красивого текста результата: "Ходы: X • Время: mm:ss"

public class ScoreTextConverter : IValueConverter // Конвертер для отображения результата
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) // Преобразование результата в строку
    {
        if (value is ScoreEntry s) // если значение — запись результата
            return $"Ходы: {s.Moves} • Время: {s.Time:mm\\:ss}";
        return "";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}
