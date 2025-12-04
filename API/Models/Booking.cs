using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoomBookingAPI.Models
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int RoomId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [MaxLength(500)]
        public string? Purpose { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "Pending"; // Pending, Confirmed, Cancelled

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        [ForeignKey("RoomId")]
        public Room Room { get; set; } = null!;

        [ForeignKey("UserId")]
        public User User { get; set; } = null!;
    }
}
