namespace LeaderboardAPI.Models;

// This is an Entity Model. Entity Framework Core looks at this exact class and 
// automatically generates a PostgreSQL table named "Matches" with matching columns.
public class Match
{
    // By naming this property "Id", Entity Framework automatically knows to make 
    // it the Primary Key for the table. It will auto-increment (1, 2, 3...) 
    // every time you save a new match to the database.
    public int Id { get; set; }
    
    // A string column to store the name of the level or map. 
    // We set it to "= string.Empty;" by default so that if you ever forget to provide 
    // a map name, it saves as a blank text string instead of throwing a "Null" crash.
    public string MapName { get; set; } = string.Empty;
    
    // An integer column to track how long the game lasted.
    public int DurationSeconds { get; set; }
    
    // A timestamp column recording exactly when the match occurred.
    // By setting it to "= DateTime.UtcNow;", C# will automatically stamp it with the 
    // exact current time (in universal standard time) the moment you create a new Match object.
    public DateTime PlayedAt { get; set; } = DateTime.UtcNow;
}