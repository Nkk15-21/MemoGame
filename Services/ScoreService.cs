using System.Text.Json;
using Microsoft.Maui.Storage;

namespace MemoGame.Services;

/// <summary>
/// Сохранение лучших результатов. Данные храним в Preferences как JSON.
/// </summary>
public record ScoreEntry(string Player, int Moves, TimeSpan Time, DateTime Date);

public class ScoreService
{
    private const string Key = "memo_scores_v1";

    public IList<ScoreEntry> GetAll()
    {
        var raw = Preferences.Get(Key, "");
        if (string.IsNullOrWhiteSpace(raw)) return new List<ScoreEntry>();
        try { return JsonSerializer.Deserialize<List<ScoreEntry>>(raw) ?? new(); }
        catch { return new List<ScoreEntry>(); }
    }

    public void Add(ScoreEntry entry)
    {
        var list = GetAll().ToList();
        list.Add(entry);
        // сортируем: сначала по количеству ходов, затем по времени
        list = list.OrderBy(e => e.Moves).ThenBy(e => e.Time).Take(50).ToList(); // ограничим топ-50
        Preferences.Set(Key, JsonSerializer.Serialize(list));
    }

    public ScoreEntry? GetBest() => GetAll().FirstOrDefault();
}
