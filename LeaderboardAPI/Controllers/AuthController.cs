using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LeaderboardAPI.Controllers;

// Tells ASP.NET that this class handles web requests and should automatically 
// format responses as JSON and map incoming data correctly.
[ApiController]

// Sets the base URL for this controller. [controller] is a variable that automatically 
// grabs the class name without the "Controller" part. So this becomes: /api/auth
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    // A private variable to hold our application settings so we can read them later.
    private readonly IConfiguration _config;

    // The constructor asks the system to "inject" the IConfiguration service when 
    // it creates this controller. This gives us access to everything inside appsettings.json.
    public AuthController(IConfiguration config)
    {
        _config = config;
    }

    // A quick, lightweight way to define the exact JSON structure we expect the Unity 
    // client to send when attempting to log in.
    public record LoginRequest(string Username, string Password);

    // Maps this specific method to handle POST requests sent to: /api/auth/login
    // [FromBody] tells it to look inside the body of the HTTP request for the JSON payload.
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        // Hardcoded check for demonstration. In a real game, you would query your 
        // PostgreSQL database here to see if the username and password match a registered user.
        if (request.Username == "admin" && request.Password == "password123")
        {
            // ==========================================
            // JWT GENERATION
            // ==========================================
            
            // 1. Grab the secret key from appsettings.json and turn it into raw bytes.
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            
            // 2. Create the cryptographic signature using that key and the HMAC SHA256 algorithm.
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // 3. Assemble the actual token. We plug in the Issuer and Audience from appsettings,
            // tell it to expire 120 minutes from right now, and attach the cryptographic signature.
            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                null, // "Claims" go here (like user ID or role), but we are skipping them for simplicity.
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            // 4. Translate the token object into the final, encrypted string format (eyJh...)
            // and send it back to the Unity client with an HTTP 200 OK status.
            return Ok(new { Token = new JwtSecurityTokenHandler().WriteToken(token) });
        }

        // If the username or password was wrong, reject the request with an HTTP 401 Unauthorized status.
        return Unauthorized("Invalid credentials");
    }
}