using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using LeaderboardAPI;
using LeaderboardAPI.Controllers;
using LeaderboardAPI.Models;

namespace LeaderboardAPI.Tests;

public class LeaderboardTests
{
    [Fact]
    public async Task SubmitScore_AddsNewScoreToDatabase()
    {
        
        var options = new DbContextOptionsBuilder<LeaderboardContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        
        using var context = new LeaderboardContext(options);
        var controller = new LeaderboardController(context);
        
      
        var request = new LeaderboardController.SubmitScoreRequest(1, 99, 5000, true);
        
      
        var result = await controller.SubmitScore(request);
        
       
        Assert.IsType<OkObjectResult>(result); // Ensure it returned a 200 OK
        Assert.Equal(1, context.MatchResults.Count()); // Ensure exactly 1 score was saved
        
        var savedScore = context.MatchResults.First();
        Assert.Equal(5000, savedScore.Score); // Ensure the score value matches
    }
}