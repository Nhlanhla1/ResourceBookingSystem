using Microsoft.AspNetCore.Mvc;
using ResourceBookingSystem.Models;
using ResourceBookingSystem.Services;

namespace ResourceBookingSystem.Controllers
{
    public class ResourceController : Controller
    {
        private readonly IResourceService _resourceService;
        private readonly IBookingService _bookingService;

        public ResourceController(IResourceService resourceService, IBookingService bookingService)
        {
            _resourceService = resourceService;
            _bookingService = bookingService;
        }

        // GET: Resource
        public async Task<IActionResult> Index()
        {
            try
            {
                var resources = await _resourceService.GetAllResourcesAsync();
                return View(resources);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading resources.";
                return View(new List<Resource>());
            }
        }

        // GET: Resource/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var resource = await _resourceService.GetResourceByIdAsync(id);
                if (resource == null)
                {
                    TempData["ErrorMessage"] = "Resource not found.";
                    return RedirectToAction(nameof(Index));
                }

                var bookings = await _bookingService.GetBookingsByResourceIdAsync(id);
                ViewBag.Bookings = bookings;

                return View(resource);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading the resource.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Resource/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Resource/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,Location,Capacity,IsAvailable")] Resource resource)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _resourceService.CreateResourceAsync(resource);
                    TempData["SuccessMessage"] = "Resource created successfully.";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while creating the resource.";
            }

            return View(resource);
        }

        // GET: Resource/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var resource = await _resourceService.GetResourceByIdAsync(id);
                if (resource == null)
                {
                    TempData["ErrorMessage"] = "Resource not found.";
                    return RedirectToAction(nameof(Index));
                }
                return View(resource);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading the resource.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Resource/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Location,Capacity,IsAvailable")] Resource resource)
        {
            if (id != resource.Id)
            {
                TempData["ErrorMessage"] = "Resource ID mismatch.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                if (ModelState.IsValid)
                {
                    await _resourceService.UpdateResourceAsync(resource);
                    TempData["SuccessMessage"] = "Resource updated successfully.";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while updating the resource.";
            }

            return View(resource);
        }

        // GET: Resource/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var resource = await _resourceService.GetResourceByIdAsync(id);
                if (resource == null)
                {
                    TempData["ErrorMessage"] = "Resource not found.";
                    return RedirectToAction(nameof(Index));
                }

                return View(resource);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading the resource.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Resource/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var result = await _resourceService.DeleteResourceAsync(id);
                if (result)
                {
                    TempData["SuccessMessage"] = "Resource deleted successfully.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Resource not found.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the resource. It may have existing bookings.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}