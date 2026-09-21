using Microsoft.EntityFrameworkCore;
using LeaderboardAPI.Models;

namespace LeaderboardAPI;

// DbContext is the core Entity Framework class. It acts as the official bridge 
// between your C# application and your PostgreSQL database.
public class LeaderboardContext : DbContext
{
    // The constructor accepts configuration options (like the database connection string 
    // we set in Program.cs) and passes them down to the base DbContext class so it knows 
    // exactly how to log in and connect to PostgreSQL.
    public LeaderboardContext(DbContextOptions<LeaderboardContext> options) : base(options) { }

    // A DbSet represents a single table in your database. 
    // Entity Framework will automatically look at your C# 'Player' model, create a 
    // matching 'Players' table in PostgreSQL, and allow you to read/write to it using C#.
    public DbSet<Player> Players { get; set; }
    
    // Maps to the 'Matches' table in the database.
    public DbSet<Match> Matches { get; set; }
    
    // Maps to the 'MatchResults' table, which is where the SubmitScore endpoint 
    // saves the incoming data from the game client.
    public DbSet<MatchResult> MatchResults { get; set; }
}