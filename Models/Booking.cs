using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int RoomId { get; set; } // Foreign Key for Room

        [Required]
        public DateTime StartDate { get; set; } // Booking start date

        [Required]
        public DateTime EndDate { get; set; } // Booking end date

        [ForeignKey("RoomId")]
        public Room Room { get; set; } // Navigation property
    }
}
