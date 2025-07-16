using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResourceBookingSystem.Models
{
    public class Booking
    {
        public int Id { get; set; }

        [Required]
        public int ResourceId { get; set; }

        [Required(ErrorMessage = "Start time is required")]
        [Display(Name = "Start Time")]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = "End time is required")]
        [Display(Name = "End Time")]
        public DateTime EndTime { get; set; }

        [Required(ErrorMessage = "Booked by is required")]
        [StringLength(100, ErrorMessage = "Booked by cannot exceed 100 characters")]
        [Display(Name = "Booked By")]
        public string BookedBy { get; set; } = string.Empty;

        [Required(ErrorMessage = "Purpose is required")]
        [StringLength(200, ErrorMessage = "Purpose cannot exceed 200 characters")]
        public string Purpose { get; set; } = string.Empty;

        [Display(Name = "Booking Date")]
        public DateTime BookingDate { get; set; } = DateTime.Now;

        // Navigation property
        [ForeignKey("ResourceId")]
        public virtual Resource Resource { get; set; } = null!;
    }
}