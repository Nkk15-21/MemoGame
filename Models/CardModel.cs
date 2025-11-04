using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MemoGame.Models;


// Модель карточки (элемент игры).
// Реализует INotifyPropertyChanged, чтобы UI обновлялся при изменениях.

public class CardModel : INotifyPropertyChanged
{
    public Guid Id { get; } = Guid.NewGuid(); // уникальный идентификатор пары
    public string Symbol { get; init; } = "";  // контент карточки (эмодзи/буква)

    private bool _isRevealed;                  // открыта ли сейчас
    public bool IsRevealed
    {
        get => _isRevealed;
        set { if (_isRevealed != value) { _isRevealed = value; OnPropertyChanged(); } }
    }

    private bool _isMatched;                   // уже найдена пара (зафиксирована)
    public bool IsMatched
    {
        get => _isMatched;
        set { if (_isMatched != value) { _isMatched = value; OnPropertyChanged(); } }
    }

    // Удобно хранить индекс в сетке (необязательно)
    public int Row { get; set; }
    public int Col { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string? prop = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
}
