using HospitalManagementAPI.Data;
using HospitalManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto.Generators;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HospitalManagementAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly HospitalContext _context;
    private readonly IConfiguration _config;

    public AuthController(HospitalContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(User user)
    {
        if (_context.Users.Any(u => u.Email == user.Email))
            return BadRequest("Email already registered.");

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash); // Hash password

        Console.WriteLine($"Received role: {user.Role}");

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return Ok(new { message = " Account created successfully." });

    }

    [HttpPost("login")]
    public IActionResult Login(User login)
    {
        var user = _context.Users.SingleOrDefault(u => u.Email == login.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(login.PasswordHash, user.PasswordHash))
            return Unauthorized("Invalid credentials");


        Console.WriteLine($"Logging in user with Role: {user.Role}");

        // ✅ Safely fetch JWT key
        var jwtKey = _config["Jwt:Key"];
        if (string.IsNullOrWhiteSpace(jwtKey))
            return StatusCode(StatusCodes.Status500InternalServerError, "JWT key is missing or not configured.");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
        new Claim(ClaimTypes.Name, user.Email),
        new Claim(ClaimTypes.Role, user.Role ?? "User")

    };

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds);

        return Ok(new
        {
            token = new JwtSecurityTokenHandler().WriteToken(token)
        });
    }

}
