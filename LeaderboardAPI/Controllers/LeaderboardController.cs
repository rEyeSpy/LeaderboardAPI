using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LeaderboardAPI.Models;

namespace LeaderboardAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeaderboardController : ControllerBase
{
    private readonly LeaderboardContext _context;

    public LeaderboardController(LeaderboardContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetLeaderboard()
    {
        var topScores = await _context.MatchResults
            .OrderByDescending(m => m.Score)
            .Take(10)
            .Select(m => new 
            {
                PlayerId = m.PlayerId,
                Score = m.Score,
                Victory = m.DidWin
            })
            .ToListAsync();

        return Ok(topScores);
    }

    // 
    public record SubmitScoreRequest(int PlayerId, int MatchId, int Score, bool DidWin);
    [Authorize]
    [HttpPost("scores")]
    public async Task<IActionResult> SubmitScore([FromBody] SubmitScoreRequest request)
    {
        var newScore = new MatchResult
        {
            PlayerId = request.PlayerId,
            MatchId = request.MatchId,
            Score = request.Score,
            DidWin = request.DidWin
        };
        
        _context.MatchResults.Add(newScore);
        await _context.SaveChangesAsync();
        
        return Ok(new { Message = "Score saved!", ScoreId = newScore.Id });
    }
}