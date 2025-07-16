using Microsoft.EntityFrameworkCore;
using ResourceBookingSystem.Data;
using ResourceBookingSystem.Models;

namespace ResourceBookingSystem.Services
{
    public class ResourceService : IResourceService
    {
        private readonly ApplicationDbContext _context;

        public ResourceService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Resource>> GetAllResourcesAsync()
        {
            return await _context.Resources.ToListAsync();
        }

        public async Task<Resource?> GetResourceByIdAsync(int id)
        {
            return await _context.Resources
                .Include(r => r.Bookings)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Resource> CreateResourceAsync(Resource resource)
        {
            _context.Resources.Add(resource);
            await _context.SaveChangesAsync();
            return resource;
        }

        public async Task<Resource> UpdateResourceAsync(Resource resource)
        {
            _context.Resources.Update(resource);
            await _context.SaveChangesAsync();
            return resource;
        }

        public async Task<bool> DeleteResourceAsync(int id)
        {
            var resource = await _context.Resources.FindAsync(id);
            if (resource == null)
                return false;

            _context.Resources.Remove(resource);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Resource>> GetAvailableResourcesAsync()
        {
            return await _context.Resources
                .Where(r => r.IsAvailable)
                .ToListAsync();
        }
    }
}