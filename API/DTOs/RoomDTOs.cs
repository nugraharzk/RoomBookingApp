using System.ComponentModel.DataAnnotations;

namespace RoomBookingAPI.DTOs
{
    public class RoomDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Capacity { get; set; }
        public string? Location { get; set; }
        public bool IsAvailable { get; set; }
        public decimal? PricePerHour { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class CreateRoomDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        [Range(1, 1000)]
        public int Capacity { get; set; }

        [MaxLength(200)]
        public string? Location { get; set; }

        public bool IsAvailable { get; set; } = true;

        [Range(0, 10000)]
        public decimal? PricePerHour { get; set; }
    }

    public class UpdateRoomDto
    {
        [MaxLength(100)]
        public string? Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [Range(1, 1000)]
        public int? Capacity { get; set; }

        [MaxLength(200)]
        public string? Location { get; set; }

        public bool? IsAvailable { get; set; }

        [Range(0, 10000)]
        public decimal? PricePerHour { get; set; }
    }
}
