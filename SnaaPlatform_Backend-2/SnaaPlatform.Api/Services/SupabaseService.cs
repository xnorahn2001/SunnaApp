using Supabase;
using SnaaPlatform.Api.Models;

namespace SnaaPlatform.Api.Services
{
    public class SupabaseService : ISupabaseService
    {
        private readonly Client _client;

        public SupabaseService(Client client)
        {
            _client = client;
        }

        public async Task<string> UploadFileAsync(string bucketName, string fileName, Stream fileStream)
        {
            using var memoryStream = new MemoryStream();
            await fileStream.CopyToAsync(memoryStream);
            var bytes = memoryStream.ToArray();

            await _client.Storage.From(bucketName).Upload(bytes, fileName);
            return await GetPublicUrlAsync(bucketName, fileName);
        }

        public async Task<string> GetPublicUrlAsync(string bucketName, string fileName)
        {
            return _client.Storage.From(bucketName).GetPublicUrl(fileName);
        }

        // Profile Operations
        public async Task<Profile?> GetProfileByEmailAsync(string email)
        {
            var response = await _client.From<Profile>()
                .Where(x => x.Email == email)
                .Get();

            return response.Models.FirstOrDefault();
        }

        public async Task<bool> CreateProfileAsync(Profile profile)
        {
            var response = await _client.From<Profile>().Insert(profile);
            return response.Models.Count > 0;
        }

        public async Task<bool> UpdateProfileAsync(Profile profile)
        {
            var response = await _client.From<Profile>().Update(profile);
            return response.Models.Count > 0;
        }

        // Project Operations
        public async Task<List<Project>> GetProjectsAsync()
        {
            var response = await _client.From<Project>().Get();
            return response.Models;
        }

        public async Task<bool> CreateProjectAsync(Project project)
        {
            var response = await _client.From<Project>().Insert(project);
            return response.Models.Count > 0;
        }

        // Order Operations
        public async Task<List<Order>> GetOrdersAsync(string userId)
        {
            var response = await _client.From<Order>()
                .Where(x => x.ClientId == userId || x.DesignerId == userId)
                .Get();
            return response.Models;
        }

        public async Task<bool> CreateOrderAsync(Order order)
        {
            var response = await _client.From<Order>().Insert(order);
            return response.Models.Count > 0;
        }

        // Consultation Operations
        public async Task<bool> CreateConsultationAsync(Consultation consultation)
        {
            var response = await _client.From<Consultation>().Insert(consultation);
            return response.Models.Count > 0;
        }
    }
}
