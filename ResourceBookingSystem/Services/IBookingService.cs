using ResourceBookingSystem.Models;

namespace ResourceBookingSystem.Services
{
    public interface IBookingService
    {
        Task<IEnumerable<Booking>> GetAllBookingsAsync();
        Task<Booking?> GetBookingByIdAsync(int id);
        Task<Booking> CreateBookingAsync(Booking booking);
        Task<Booking> UpdateBookingAsync(Booking booking);
        Task<bool> DeleteBookingAsync(int id);
        Task<IEnumerable<Booking>> GetBookingsByResourceIdAsync(int resourceId);
        Task<IEnumerable<Booking>> GetUpcomingBookingsAsync();
        Task<bool> IsResourceAvailableAsync(int resourceId, DateTime startTime, DateTime endTime, int? excludeBookingId = null);
    }
}