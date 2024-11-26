using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class Room
    {
        [Key]
        public int Id { get; set; } // Primary Key

        [Required]
        [MaxLength(50)]
        public string RoomNumber { get; set; } = string.Empty; // Unique room number within a hotel

        [Required]
        [Range(1, 10)]
        public int Capacity { get; set; } // Maximum number of guests allowed

        [Required]
        public decimal PricePerNight { get; set; } // Price per night for the room

        [Required]
        public bool IsAvailable { get; set; } = true; // Availability status

        [MaxLength(200)]
        public string Description { get; set; } = string.Empty; // Optional description of the room
    }
}
