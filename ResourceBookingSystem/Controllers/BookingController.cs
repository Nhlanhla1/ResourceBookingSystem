using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ResourceBookingSystem.Models;
using ResourceBookingSystem.Services;

namespace ResourceBookingSystem.Controllers
{
    public class BookingController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly IResourceService _resourceService; // Assuming you have this service

        public BookingController(IBookingService bookingService, IResourceService resourceService)
        {
            _bookingService = bookingService;
            _resourceService = resourceService;
        }

        public async Task<IActionResult> Index()
        {
            var bookings = await _bookingService.GetAllBookingsAsync();
            return View(bookings);
        }

        public async Task<IActionResult> Create()
        {
            await PopulateResourceDropDown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Booking booking)
        {
            try
            {
                // Server-side validation for time logic
                if (booking.StartTime >= booking.EndTime)
                {
                    ModelState.AddModelError("EndTime", "End time must be after start time");
                }

                if (booking.StartTime < DateTime.Now)
                {
                    ModelState.AddModelError("StartTime", "Start time cannot be in the past");
                }

                // Check for minimum booking duration (e.g., 30 minutes)
                if (booking.EndTime.Subtract(booking.StartTime).TotalMinutes < 30)
                {
                    ModelState.AddModelError("EndTime", "Booking must be at least 30 minutes long");
                }

                // Check resource availability
                if (!await _bookingService.IsResourceAvailableAsync(booking.ResourceId, booking.StartTime, booking.EndTime))
                {
                    ModelState.AddModelError("", "The selected resource is not available for the specified time period");
                }

                if (ModelState.IsValid)
                {
                    // Set booking date to current time
                    booking.BookingDate = DateTime.Now;

                    await _bookingService.CreateBookingAsync(booking);
                    TempData["SuccessMessage"] = "Booking created successfully!";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                // Log the exception (you should use a proper logging framework)
                ModelState.AddModelError("", "An error occurred while creating the booking. Please try again.");
                // In development, you might want to show the actual error:
                // ModelState.AddModelError("", ex.Message);
            }

            // If we got this far, something failed, redisplay form
            await PopulateResourceDropDown(booking.ResourceId);
            return View(booking);
        }

        private async Task PopulateResourceDropDown(object selectedResource = null)
        {
            var resources = await _resourceService.GetAllResourcesAsync();
            ViewBag.ResourceId = new SelectList(resources, "Id", "Name", selectedResource);
        }

        public async Task<IActionResult> Details(int id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            return View(booking);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            await PopulateResourceDropDown(booking.ResourceId);
            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Booking booking)
        {
            if (id != booking.Id)
            {
                return NotFound();
            }

            try
            {
                // Server-side validation for time logic
                if (booking.StartTime >= booking.EndTime)
                {
                    ModelState.AddModelError("EndTime", "End time must be after start time");
                }

                if (booking.StartTime < DateTime.Now)
                {
                    ModelState.AddModelError("StartTime", "Start time cannot be in the past");
                }

                // Check for minimum booking duration (e.g., 30 minutes)
                if (booking.EndTime.Subtract(booking.StartTime).TotalMinutes < 30)
                {
                    ModelState.AddModelError("EndTime", "Booking must be at least 30 minutes long");
                }

                // Check resource availability (exclude current booking)
                if (!await _bookingService.IsResourceAvailableAsync(booking.ResourceId, booking.StartTime, booking.EndTime, booking.Id))
                {
                    ModelState.AddModelError("", "The selected resource is not available for the specified time period");
                }

                if (ModelState.IsValid)
                {
                    await _bookingService.UpdateBookingAsync(booking);
                    TempData["SuccessMessage"] = "Booking updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while updating the booking. Please try again.");
            }

            await PopulateResourceDropDown(booking.ResourceId);
            return View(booking);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            return View(booking);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _bookingService.DeleteBookingAsync(id);
                TempData["SuccessMessage"] = "Booking deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the booking.";
            }
            return RedirectToAction(nameof(Index));
        }

        // API endpoint to check availability
        [HttpGet]
        public async Task<IActionResult> CheckAvailability(int resourceId, DateTime startTime, DateTime endTime, int? excludeBookingId = null)
        {
            var isAvailable = await _bookingService.IsResourceAvailableAsync(resourceId, startTime, endTime, excludeBookingId);
            return Json(new { available = isAvailable });
        }
    }
}