using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using LeaderboardAPI;
using LeaderboardAPI.Controllers;
using LeaderboardAPI.Models;

namespace LeaderboardAPI.Tests;

public class LeaderboardTests
{
    // [Fact] tells the xUnit test runner that this specific method is an automated test.
    [Fact]
    public async Task SubmitScore_AddsNewScoreToDatabase()
    {
        // ==========================================
        // 1. ARRANGE (Set up the environment)
        // ==========================================
        
        // Configures Entity Framework to use a fake, temporary database in your computer's RAM.
        // Guid.NewGuid() generates a random name for it so that if you run 100 tests at once, 
        // they each get their own isolated, blank database and don't overwrite each other's data.
        var options = new DbContextOptionsBuilder<LeaderboardContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        
        // Creates the actual connection to our fake in-memory database.
        // The 'using' keyword ensures the database is properly deleted from RAM after the test finishes.
        using var context = new LeaderboardContext(options);
        
        // Creates an instance of your API controller, handing it the fake database instead of the real PostgreSQL one.
        var controller = new LeaderboardController(context);
        
        // Creates a dummy payload that perfectly mimics the JSON data your Unity game will send over the network.
        // (PlayerId: 1, MatchId: 99, Score: 5000, DidWin: true)
        var request = new LeaderboardController.SubmitScoreRequest(1, 99, 5000, true);
        
        
        // ==========================================
        // 2. ACT (Execute the code being tested)
        // ==========================================
        
        // Simulates an HTTP POST request hitting the endpoint and waits for it to finish processing.
        var result = await controller.SubmitScore(request);
        
        
        // ==========================================
        // 3. ASSERT (Verify the results are correct)
        // ==========================================
        
        // Checks the HTTP response. If the controller returns anything other than an "Ok" (HTTP 200), the test fails.
        Assert.IsType<OkObjectResult>(result); 
        
        // Looks inside the fake database to ensure exactly 1 new row was added to the MatchResults table.
        Assert.Equal(1, context.MatchResults.Count()); 
        
        // Grabs that newly saved row from the database so we can inspect it.
        var savedScore = context.MatchResults.First();
        
        // Confirms that the database successfully saved the score as 5000, not 0 or null.
        Assert.Equal(5000, savedScore.Score); 
    }
}