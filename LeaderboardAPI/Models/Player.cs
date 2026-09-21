namespace LeaderboardAPI.Models;

// This Entity Model maps to the "Players" table in your PostgreSQL database. 
// It represents a single registered user in your game.
public class Player
{
    // The Primary Key. Entity Framework automatically configures this column to auto-increment 
    // (1, 2, 3...) every time a new player account is created in the database.
    public int Id { get; set; }
    
    // A string column to store the player's display name.
    // We initialize it to "= string.Empty;" so that if a player object is ever created without a name,
    // it defaults to a safe, blank string instead of a 'null' value. This prevents null reference crashes.
    public string Username { get; set; } = string.Empty;
    
    // A timestamp column recording the exact moment this player account was created.
    // By setting it to "= DateTime.UtcNow;", C# automatically fills this in with the current server time 
    // (in universal standard time) the instant the object is instantiated in memory.
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}