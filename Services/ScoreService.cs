using System.Text.Json;
using Microsoft.Maui.Storage;

namespace MemoGame.Services;


// Сохранение лучших результатов. Данные храним в Preferences как JSON.

public record ScoreEntry(string Player, int Moves, TimeSpan Time, DateTime Date);// запись результата

public class ScoreService// сервис для работы с результатами
{
    private const string Key = "memo_scores_v1";

    public IList<ScoreEntry> GetAll()// получить все результаты
    {
        var raw = Preferences.Get(Key, "");
        if (string.IsNullOrWhiteSpace(raw)) return new List<ScoreEntry>();// если нет данных, вернуть пустой список
        try { return JsonSerializer.Deserialize<List<ScoreEntry>>(raw) ?? new(); }// десериализация из JSON
        catch { return new List<ScoreEntry>(); }
    }

    public void Add(ScoreEntry entry)// добавить новый результат
    {
        var list = GetAll().ToList();
        list.Add(entry);
        // сортируем: сначала по количеству ходов, затем по времени
        list = list.OrderBy(e => e.Moves).ThenBy(e => e.Time).Take(50).ToList(); // ограничим топ-50
        Preferences.Set(Key, JsonSerializer.Serialize(list));
    }

    public ScoreEntry? GetBest() => GetAll().FirstOrDefault();
}
