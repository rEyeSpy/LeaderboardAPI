namespace LeaderboardAPI.Models;

public class Match
{
    public int Id { get; set; }
    public string MapName { get; set; } = string.Empty;
    public int DurationSeconds { get; set; }
    public DateTime PlayedAt { get; set; } = DateTime.UtcNow;
}