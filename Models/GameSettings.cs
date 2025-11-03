namespace MemoGame.Models;

/// <summary>
/// Настройки игры (размерность поля, набор символов и т.п.).
/// </summary>
public class GameSettings
{
    public int Rows { get; set; } = 4;   // 4x4 — 8 пар
    public int Cols { get; set; } = 4;

    // Можно добавить уровни сложности, таймер и т.д.
}
