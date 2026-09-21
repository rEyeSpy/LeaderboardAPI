using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using LeaderboardAPI; 

// Creates the web application builder, giving us access to configuration files (appsettings.json) 
// and allowing us to register system services before the app actually starts.
var builder = WebApplication.CreateBuilder(args);

// Tells the app to look for and enable our Controller classes (like LeaderboardController and AuthController).
builder.Services.AddControllers();

// ==========================================
// JWT AUTHENTICATION SETUP
// ==========================================
// Registers the authentication system and tells it to expect "Bearer" tokens (JWTs) in the HTTP headers.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // This block defines exactly how the server decides if an incoming token is fake or legitimate.
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,   // Ensure the token was created by our specific server.
            ValidateAudience = true, // Ensure the token was meant for our specific game client.
            ValidateLifetime = true, // Ensure the token hasn't expired yet.
            ValidateIssuerSigningKey = true, // Ensure the token's cryptographic signature is valid.
            
            // Reads the acceptable values from appsettings.json to compare against the incoming token.
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            
            // Re-creates the secret key used to sign the tokens. If a hacker tries to forge a token, 
            // the math won't match this secret key, and the server will reject it.
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

// Enables the system that checks permissions (the [Authorize] attribute we put on the POST endpoint).
builder.Services.AddAuthorization();

// ==========================================
// DATABASE SETUP
// ==========================================
// Registers our LeaderboardContext with the system and tells it to connect to PostgreSQL (UseNpgsql).
// It grabs the exact database password and port from the "DefaultConnection" string in appsettings.json.
builder.Services.AddDbContext<LeaderboardContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Locks in all the services we just registered and builds the actual web application.
var app = builder.Build();


// ==========================================
// HTTP REQUEST PIPELINE (Order is critical here)
// ==========================================

// Automatically redirects any unsecured HTTP requests to encrypted HTTPS.
app.UseHttpsRedirection();

// 1. Authentication MUST come first. The server reads the token and figures out WHO the user is.
app.UseAuthentication(); 

// 2. Authorization comes second. Now that the server knows who the user is, it checks if they are ALLOWED to access the endpoint.
app.UseAuthorization();

// Connects incoming web URLs (like /api/leaderboard/scores) to the actual C# methods in your Controllers.
app.MapControllers();

// Starts the server and begins actively listening for incoming web requests.
app.Run();