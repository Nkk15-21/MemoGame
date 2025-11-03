using MemoGame.Models;
using MemoGame.Services;

namespace MemoGame.Views;

public partial class GamePage : ContentPage // Страница игры
{
    private readonly GameEngine _engine; // Игровой движок

    // Кэш кнопок, чтобы быстро обновлять UI при изменении карточек
    private readonly Dictionary<CardModel, Button> _buttons = new();

    public GamePage(GameSettings settings, Player player)
    {
        InitializeComponent();

        // Создаём игровой движок и подписываемся на события
        _engine = new GameEngine(settings, player);
        _engine.BoardGenerated += BuildBoard;
        _engine.CardUpdated += OnCardUpdated;
        _engine.GameFinished += OnGameFinished;

        PlayerLabel.Text = $"Игрок: {player.Name}"; // Показываем имя игрока
        Start();
    }

    private void Start() // Запуск новой игры
    {
        _buttons.Clear(); // Очищаем кэш кнопок
        BoardGrid.Children.Clear();
        BoardGrid.RowDefinitions.Clear();
        BoardGrid.ColumnDefinitions.Clear();

        // Генерация новой игры
        _engine.StartNew();

        // Обновляем статусную строку
        MovesLabel.Text = $"Ходы: 0";
        MatchesLabel.Text = $"Пары: 0 / {_engine.Cards.Count / 2}";
    }

    private void BuildBoard()
    {
        // Конструируем сетку: Rows x Cols
        for (int r = 0; r < _engine.Settings.Rows; r++)
            BoardGrid.RowDefinitions.Add(new RowDefinition(GridLength.Star));
        for (int c = 0; c < _engine.Settings.Cols; c++)
            BoardGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));

        // На каждую модель карточки — своя кнопка
        foreach (var card in _engine.Cards)
        {
            var btn = new Button
            {
                Style = (Style)Application.Current!.Resources["CardButtonStyle"],
                Text = "?", // изначально — скрыто
                BackgroundColor = (Color)Application.Current!.Resources["CardColor"]
            };

            // Tap: если успешно, запустим лёгкую анимацию "пружинка"
            btn.Clicked += async (_, __) =>
            {
                if (await _engine.TapAsync(card))
                {
                    await btn.ScaleTo(1.08, 75);
                    await btn.ScaleTo(1.0, 75);
                }
            };

            // Позиция в сетке
            Grid.SetRow(btn, card.Row);
            Grid.SetColumn(btn, card.Col);

            // Сохраняем связь
            _buttons[card] = btn;

            BoardGrid.Children.Add(btn);
        }
    }

    private void OnCardUpdated(CardModel card)
    {
        // Обновляем UI одной карточки
        if (!_buttons.TryGetValue(card, out var btn)) return;

        // Если карточка открыта — показываем символ; иначе — "?"
        btn.Text = card.IsRevealed || card.IsMatched ? card.Symbol : "?";

        // Найденные пары делаем полупрозрачными и отключаем кнопку
        if (card.IsMatched)
        {
            btn.Opacity = 0.6;
            btn.IsEnabled = false;
        }

        // Обновляем статус
        MatchesLabel.Text = $"Пары: {_engine.Matches} / {_engine.Cards.Count / 2}";
        MovesLabel.Text = $"Ходы: {_engine.Moves}";
    }

    private async void OnGameFinished(TimeSpan time, int moves)
    {
        // Сохраняем результат в сервисе рекордов
        App.ScoreService.Add(new ScoreEntry(
            Player: _engine.Player.Name,
            Moves: moves,
            Time: time,
            Date: DateTime.Now));

        await DisplayAlert("Готово!", $"Вы нашли все пары за {time:mm\\:ss} и {moves} ход(ов).", "Ок");
    }

    private void RestartClicked(object sender, EventArgs e) => Start();

    private async void BackClicked(object sender, EventArgs e) => await Navigation.PopAsync();
}
