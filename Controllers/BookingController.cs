using Backend.Data;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers

{
    [Route("api/Booking")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public BookingController(ApplicationDBContext context)
        {
            _context = context;
        }

        // Get all bookings
        [HttpGet]
        public IActionResult GetAllBookings()
        {
            var bookings = _context.Bookings.ToList();
            return Ok(bookings);
        }

        // Get booking by ID
        [HttpGet("{id}")]
        public IActionResult GetBookingById([FromRoute]int id)
        {
            var booking = _context.Bookings.Find(id);
            if (booking == null)
            {
                return NotFound("Booking not found.");
            }
            return Ok(booking);
        }

        // Create a new booking
        [HttpPost]
        public IActionResult CreateBooking([FromBody] Booking newBooking)
        {
            if (newBooking == null)
            {
            return BadRequest("Invalid booking data.");
            }

            _context.Bookings.Add(newBooking);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetBookingById), new { id = newBooking.Id }, newBooking);
        }

        // Cancel a booking
        [HttpDelete("{id}")]
        public IActionResult CancelBooking([FromRoute] int id)
        {
            var booking = _context.Bookings.Find(id);
            if (booking == null)
            {
            return NotFound("Booking not found.");
            }

            _context.Bookings.Remove(booking);
            _context.SaveChanges();

            return NoContent();
        }

       
    }
}

