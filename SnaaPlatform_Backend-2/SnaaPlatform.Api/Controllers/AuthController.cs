using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SnaaPlatform.Api.DTOs;
using SnaaPlatform.Api.Models;
using SnaaPlatform.Api.Services;
using System.Security.Claims;

namespace SnaaPlatform.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ISupabaseService _supabaseService;
        private readonly IAuthService _authService;

        public AuthController(ISupabaseService supabaseService, IAuthService authService)
        {
            _supabaseService = supabaseService;
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var existingProfile = await _supabaseService.GetProfileByEmailAsync(request.Email);
            if (existingProfile != null)
                return BadRequest(new { message = "Email already exists" });

            var profile = new Profile
            {
                Id = Guid.NewGuid().ToString(),
                Email = request.Email,
                FullName = request.FullName,
                AccountType = request.AccountType,
                CommercialRegistration = request.AccountType == "Facility" ? request.CommercialRegistration : null,
                Specialization = request.AccountType == "Individual" ? request.Specialization : null,
                PasswordHash = _authService.HashPassword(request.Password),
                CreatedAt = DateTime.UtcNow
            };

            var success = await _supabaseService.CreateProfileAsync(profile);
            if (!success)
                return StatusCode(500, new { message = "Error creating profile" });

            var token = _authService.GenerateJwtToken(profile);
            return Ok(new AuthResponse
            {
                Token = token,
                Email = profile.Email,
                FullName = profile.FullName,
                AccountType = profile.AccountType
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var profile = await _supabaseService.GetProfileByEmailAsync(request.Email);
            if (profile == null || !_authService.VerifyPassword(request.Password, profile.PasswordHash))
                return Unauthorized(new { message = "Invalid email or password" });

            var token = _authService.GenerateJwtToken(profile);
            return Ok(new AuthResponse
            {
                Token = token,
                Email = profile.Email,
                FullName = profile.FullName,
                AccountType = profile.AccountType
            });
        }

        [Authorize]
        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserRequest request)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(userEmail))
                return Unauthorized();

            var profile = await _supabaseService.GetProfileByEmailAsync(userEmail);
            if (profile == null)
                return NotFound(new { message = "Profile not found" });

            profile.FullName = request.FullName;
            if (profile.AccountType == "Facility")
            {
                profile.CommercialRegistration = request.CommercialRegistration;
            }
            else
            {
                profile.Specialization = request.Specialization;
            }

            var success = await _supabaseService.UpdateProfileAsync(profile);
            if (!success)
                return StatusCode(500, new { message = "Error updating profile" });

            return Ok(new { message = "Profile updated successfully", profile = new { profile.FullName, profile.AccountType, profile.CommercialRegistration, profile.Specialization } });
        }
    }
}
