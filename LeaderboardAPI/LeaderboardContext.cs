using Microsoft.EntityFrameworkCore;
using LeaderboardAPI.Models;

namespace LeaderboardAPI; // Removed .Data

public class LeaderboardContext : DbContext
{
    public LeaderboardContext(DbContextOptions<LeaderboardContext> options) : base(options) { }

    public DbSet<Player> Players { get; set; }
    public DbSet<Match> Matches { get; set; }
    public DbSet<MatchResult> MatchResults { get; set; }
}