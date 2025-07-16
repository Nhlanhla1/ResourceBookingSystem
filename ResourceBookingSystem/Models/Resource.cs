using System.ComponentModel.DataAnnotations;

namespace ResourceBookingSystem.Models
{
    public class Resource
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "Location cannot exceed 200 characters")]
        public string Location { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Capacity must be a positive number")]
        public int Capacity { get; set; }

        public bool IsAvailable { get; set; } = true;

        // Navigation property
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}