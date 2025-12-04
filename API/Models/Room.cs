using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoomBookingAPI.Models
{
    public class Room
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        public int Capacity { get; set; }

        [MaxLength(200)]
        public string? Location { get; set; }

        public bool IsAvailable { get; set; } = true;

        [Column(TypeName = "decimal(10,2)")]
        public decimal? PricePerHour { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Navigation property
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
