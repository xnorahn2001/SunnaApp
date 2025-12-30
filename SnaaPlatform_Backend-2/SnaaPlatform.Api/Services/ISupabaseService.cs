using SnaaPlatform.Api.Models;

namespace SnaaPlatform.Api.Services
{
    public interface ISupabaseService
    {
        Task<string> UploadFileAsync(string bucketName, string fileName, Stream fileStream);
        Task<string> GetPublicUrlAsync(string bucketName, string fileName);
        
        // Profile Operations
        Task<Profile?> GetProfileByEmailAsync(string email);
        Task<bool> CreateProfileAsync(Profile profile);
        Task<bool> UpdateProfileAsync(Profile profile);

        // Project Operations
        Task<List<Project>> GetProjectsAsync();
        Task<bool> CreateProjectAsync(Project project);

        // Order Operations
        Task<List<Order>> GetOrdersAsync(string userId);
        Task<bool> CreateOrderAsync(Order order);

        // Consultation Operations
        Task<bool> CreateConsultationAsync(Consultation consultation);
    }
}
