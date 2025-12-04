using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoomBookingAPI.Data;
using RoomBookingAPI.DTOs;
using RoomBookingAPI.Models;
using RoomBookingAPI.Services;
using BCrypt.Net;
using Swashbuckle.AspNetCore.Annotations;

namespace RoomBookingAPI.Controllers
{
    /// <summary>
    /// Handles user authentication and registration
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IJwtService _jwtService;

        public AuthController(ApplicationDbContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        /// <summary>
        /// Register a new user account
        /// </summary>
        /// <param name="dto">User registration details</param>
        /// <returns>Authentication token and user information</returns>
        /// <response code="200">User successfully registered</response>
        /// <response code="400">Invalid input or user already exists</response>
        [HttpPost("register")]
        [SwaggerOperation(
            Summary = "Register a new user",
            Description = "Creates a new user account and returns a JWT token for authentication",
            Tags = new[] { "Authentication" }
        )]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AuthResponseDto>> Register(RegisterDto dto)
        {
            // Check if user already exists
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email || u.Username == dto.Username))
            {
                return BadRequest(new { message = "User with this email or username already exists" });
            }

            // Create new user
            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = "User",
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Generate token
            var token = _jwtService.GenerateToken(user);

            return Ok(new AuthResponseDto
            {
                Token = token,
                User = new UserDto
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    Role = user.Role
                }
            });
        }

        /// <summary>
        /// Authenticate a user and get access token
        /// </summary>
        /// <param name="dto">Login credentials (email/username and password)</param>
        /// <returns>Authentication token and user information</returns>
        /// <response code="200">Login successful</response>
        /// <response code="401">Invalid credentials</response>
        [HttpPost("login")]
        [SwaggerOperation(
            Summary = "Login user",
            Description = "Authenticates a user with email/username and password, returns a JWT token",
            Tags = new[] { "Authentication" }
        )]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginDto dto)
        {
            // Find user by email or username
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == dto.EmailOrUsername || u.Username == dto.EmailOrUsername);

            if (user == null)
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }

            // Verify password
            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }

            // Generate token
            var token = _jwtService.GenerateToken(user);

            return Ok(new AuthResponseDto
            {
                Token = token,
                User = new UserDto
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    Role = user.Role
                }
            });
        }
    }
}
