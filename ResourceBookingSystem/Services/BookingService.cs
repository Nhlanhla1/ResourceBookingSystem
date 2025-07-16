using Microsoft.EntityFrameworkCore;
using ResourceBookingSystem.Data;
using ResourceBookingSystem.Models;

namespace ResourceBookingSystem.Services
{
    public class BookingService : IBookingService
    {
        private readonly ApplicationDbContext _context;

        public BookingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
        {
            return await _context.Bookings
                .Include(b => b.Resource)
                .OrderBy(b => b.StartTime)
                .ToListAsync();
        }

        public async Task<Booking?> GetBookingByIdAsync(int id)
        {
            return await _context.Bookings
                .Include(b => b.Resource)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Booking> CreateBookingAsync(Booking booking)
        {
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            return booking;
        }

        public async Task<Booking> UpdateBookingAsync(Booking booking)
        {
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
            return booking;
        }

        public async Task<bool> DeleteBookingAsync(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
                return false;

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Booking>> GetBookingsByResourceIdAsync(int resourceId)
        {
            return await _context.Bookings
                .Where(b => b.ResourceId == resourceId && b.EndTime >= DateTime.Now)
                .OrderBy(b => b.StartTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetUpcomingBookingsAsync()
        {
            return await _context.Bookings
                .Include(b => b.Resource)
                .Where(b => b.StartTime >= DateTime.Now)
                .OrderBy(b => b.StartTime)
                .ToListAsync();
        }

        public async Task<bool> IsResourceAvailableAsync(int resourceId, DateTime startTime, DateTime endTime, int? excludeBookingId = null)
        {
            var conflictingBookings = await _context.Bookings
                .Where(b => b.ResourceId == resourceId &&
                           b.Id != excludeBookingId &&
                           ((startTime >= b.StartTime && startTime < b.EndTime) ||
                            (endTime > b.StartTime && endTime <= b.EndTime) ||
                            (startTime <= b.StartTime && endTime >= b.EndTime)))
                .AnyAsync();

            return !conflictingBookings;
        }
    }
}