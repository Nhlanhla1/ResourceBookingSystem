using ResourceBookingSystem.Models;

namespace ResourceBookingSystem.Services
{
    public interface IResourceService
    {
        Task<IEnumerable<Resource>> GetAllResourcesAsync();
        Task<Resource?> GetResourceByIdAsync(int id);
        Task<Resource> CreateResourceAsync(Resource resource);
        Task<Resource> UpdateResourceAsync(Resource resource);
        Task<bool> DeleteResourceAsync(int id);
        Task<IEnumerable<Resource>> GetAvailableResourcesAsync();
    }
}