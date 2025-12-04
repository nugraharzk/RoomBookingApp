using System.ComponentModel.DataAnnotations;

namespace RoomBookingAPI.DTOs
{
    public class BookingDto
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public int UserId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? Purpose { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public RoomDto? Room { get; set; }
        public UserDto? User { get; set; }
    }

    public class CreateBookingDto
    {
        [Required]
        public int RoomId { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [MaxLength(500)]
        public string? Purpose { get; set; }
    }

    public class UpdateBookingDto
    {
        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        [MaxLength(500)]
        public string? Purpose { get; set; }

        [MaxLength(50)]
        public string? Status { get; set; }
    }
}
