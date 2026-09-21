namespace LeaderboardAPI.Models;

public class MatchResult
{
    public int Id { get; set; }
    public int PlayerId { get; set; }
    public int MatchId { get; set; }
    public int Score { get; set; }
    public bool DidWin { get; set; }
    
    // Navigation properties to link the tables
    public Player? Player { get; set; }
    public Match? Match { get; set; }
}