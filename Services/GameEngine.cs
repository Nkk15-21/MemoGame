using MemoGame.Models;
using System.Diagnostics;

namespace MemoGame.Services;

/// <summary>
/// Игровой движок "Memo": отвечает за генерацию поля, логику совпадений, счёт.
/// </summary>
public class GameEngine
{
    public GameSettings Settings { get; }                       // настройки игры
    public Player Player { get; }                               // игрок

    public List<CardModel> Cards { get; private set; } = new(); // все карточки
    public int Moves { get; private set; } = 0;                  // количество ходов (попыток)
    public int Matches { get; private set; } = 0;                // найденные пары

    private CardModel? _first;                                   // первая выбранная карточка
    private CardModel? _second;                                  // вторая выбранная карточка
    public bool IsBusy { get; private set; } = false;            // блокировка на время задержки/анимации

    private readonly Stopwatch _sw = new();                      // время партии

    public event Action<CardModel>? CardUpdated;                 // оповещение UI
    public event Action? BoardGenerated;                         // оповещение — поле готово
    public event Action<TimeSpan, int>? GameFinished;            // конец игры (время, ходы)

    public GameEngine(GameSettings settings, Player player)     // конструктор
    {
        Settings = settings;
        Player = player;
    }

    //Запуск новой игры
    public void StartNew()
    {
        _sw.Restart();// запуск таймера
        Moves = 0;// сброс количества ходов
        Matches = 0;// сброс количества найденных пар
        _first = _second = null;// сброс выбранных карточек
        IsBusy = false;// снятие блокировки
        GenerateBoard();// генерация игрового поля
        BoardGenerated?.Invoke();// оповещение, что поле готово
    }

    private void GenerateBoard()// генерация игрового поля
    {
        // Набор эмодзи (16 уникальных — хватит на поле 4x4 => 8 пар)
        var symbols = new List<string>
        {
            "🍎","🍌","🍇","🍉","🥝","🍓","🍍","🍑",
            "🚗","✈️","🚀","🚲","🏀","🎸","🎧","🎲",
            "🐶","🐱","🦊","🐼","🐸","🦄","🐢","🐙"
        };

        var pairsNeeded = (Settings.Rows * Settings.Cols) / 2;
        var chosen = symbols.Take(pairsNeeded).ToList();

        // создаём пары (каждый символ дважды)
        var all = new List<CardModel>();
        foreach (var s in chosen)
        {
            all.Add(new CardModel { Symbol = s });
            all.Add(new CardModel { Symbol = s });
        }

        // перемешиваем
        var rnd = new Random();
        all = all.OrderBy(_ => rnd.Next()).ToList();

        // проставим координаты (для информации)
        for (int i = 0; i < all.Count; i++)
        {
            all[i].Row = i / Settings.Cols;
            all[i].Col = i % Settings.Cols;
            all[i].IsMatched = false;
            all[i].IsRevealed = false;
        }

        Cards = all;
    }

    /// <summary>
    /// Нажатие на карточку. Возвращает false, если ход невозможен (блок или уже открыта).
    /// </summary>
    public async Task<bool> TapAsync(CardModel card)
    {
        if (IsBusy) return false;            // ждём завершения предыдущего хода
        if (card.IsMatched || card.IsRevealed) return false; // нельзя нажимать найденные/открытые

        // Открываем карточку
        card.IsRevealed = true;
        CardUpdated?.Invoke(card);

        if (_first == null)
        {
            _first = card;
            return true;
        }

        if (_second == null)
        {
            _second = card;
            Moves++;

            // Если символы равны — это пара
            if (_first.Symbol == _second.Symbol)
            {
                _first.IsMatched = _second.IsMatched = true;
                CardUpdated?.Invoke(_first);
                CardUpdated?.Invoke(_second);
                Matches++;

                // Все пары найдены? Завершение
                if (Matches == Cards.Count / 2)
                {
                    _sw.Stop();
                    GameFinished?.Invoke(_sw.Elapsed, Moves);
                }

                _first = _second = null;
                return true;
            }

            // Иначе — задержка и закрыть обе
            IsBusy = true;
            await Task.Delay(800); // маленькая пауза, чтобы игрок увидел вторую карту
            _first.IsRevealed = _second.IsRevealed = false;
            CardUpdated?.Invoke(_first);
            CardUpdated?.Invoke(_second);
            _first = _second = null;
            IsBusy = false;
            return true;
        }

        return false;
    }
}
