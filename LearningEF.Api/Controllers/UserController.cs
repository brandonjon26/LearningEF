using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LearningEF.Api.DTOs;

namespace LearningEF.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        // Inject IConfiguration to access the JWT settings
        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto login)
        {
            // HARDCODED USER CHECK (FOR REFERENCE ONLY)
            // In a real app, we will use UserService to query a DB
            if (login.Username != "testuser" || login.Password != "password")
            {
                return Unauthorized(new { Message = "Invalid username or password." });
            }

            // GENERATE JWT TOKEN
            string token = GenerateJwtToken(login.Username);

            // RETURN TOKEN
            return Ok(new { token });
        }

        private string GenerateJwtToken(string username)
        {
            // Retrieve configuration values
            var secretKey = _configuration["Jwt:SecretKey"] ?? throw new InvalidOperationException("JWT Secret Key not found.");
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];

            // Define the claims (payload) for the token
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, "Admin"), // Example role
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create the token structure
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30), // Token valid for 30 minutes
                signingCredentials: credentials);

            // Convert the token object to a string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
