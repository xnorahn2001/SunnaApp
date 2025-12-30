using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SnaaPlatform.Api.Models;
using SnaaPlatform.Api.Services;
using System.Security.Claims;

namespace SnaaPlatform.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProjectController : ControllerBase
    {
        private readonly ISupabaseService _supabaseService;

        public ProjectController(ISupabaseService supabaseService)
        {
            _supabaseService = supabaseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProjects()
        {
            var projects = await _supabaseService.GetProjectsAsync();
            return Ok(projects);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] Project project)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            project.OwnerId = userId;
            project.CreatedAt = DateTime.UtcNow;

            var success = await _supabaseService.CreateProjectAsync(project);
            if (!success) return StatusCode(500, new { message = "Error creating project" });

            return Ok(project);
        }
    }
}
