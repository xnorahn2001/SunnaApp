using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SnaaPlatform.Api.Services;

namespace SnaaPlatform.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FileController : ControllerBase
    {
        private readonly ISupabaseService _supabaseService;

        public FileController(ISupabaseService supabaseService)
        {
            _supabaseService = supabaseService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { message = "No file uploaded" });

            try
            {
                var fileName = $"{Guid.NewGuid()}_{file.FileName}";
                var bucketName = "designs"; // تأكد من إنشاء هذا الباكت في Supabase

                using var stream = file.OpenReadStream();
                var publicUrl = await _supabaseService.UploadFileAsync(bucketName, fileName, stream);

                return Ok(new { url = publicUrl, fileName = fileName });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error uploading file: {ex.Message}" });
            }
        }
    }
}
