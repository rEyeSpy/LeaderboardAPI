namespace LeaderboardAPI.Models;

// This Entity Model serves as the central junction box of your database. 
// It maps to the "MatchResults" table and connects a specific score to both the Player and the Match.
public class MatchResult
{
    // The Primary Key. Entity Framework auto-increments this (1, 2, 3...) for every new score submitted.
    public int Id { get; set; }
    
    // A Foreign Key. By specifically naming it "PlayerId", Entity Framework automatically knows 
    // this integer must match an existing 'Id' over in the 'Players' table.
    public int PlayerId { get; set; }
    
    // A Foreign Key linking this specific result to a game session in the 'Matches' table.
    public int MatchId { get; set; }
    
    // The numerical score the player achieved (for example, points accumulated from defeating enemies from your spawner).
    public int Score { get; set; }
    
    // A true/false flag tracking whether the level was successfully completed. This is perfect for 
    // hooking up directly to the win conditions you've been building in your Unity game client.
    public bool DidWin { get; set; }
    
    // ==========================================
    // NAVIGATION PROPERTIES
    // ==========================================
    // These do NOT become actual columns in the database. Instead, they tell Entity Framework 
    // how the tables logically relate to each other. 
    //
    // Because these exist, you can write C# code in your controllers like 'matchResult.Player.Username' 
    // to seamlessly pull the player's name without having to write a complex manual SQL JOIN.
    // 
    // The '?' makes them nullable, which prevents the compiler from complaining that they are 
    // empty when you first create the object before saving it to the database.
    public Player? Player { get; set; }
    public Match? Match { get; set; }
}