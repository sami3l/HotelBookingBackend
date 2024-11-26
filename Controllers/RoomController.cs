using Backend.Data;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [Route("api/rooms")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public RoomController(ApplicationDBContext context)
        {
            _context = context;
        }

        // Get all rooms
        [HttpGet]
        public IActionResult GetAllRooms()
        {
            var rooms = _context.Rooms.ToList();
            return Ok(rooms);
        }

        // Get room by ID
        [HttpGet("{id}")]
        public IActionResult GetRoomById(int id)
        {
            var room = _context.Rooms.Find(id);
            if (room == null)
            {
                return NotFound("Room not found.");
            }
            return Ok(room);
        }

        // Add a new room
        [HttpPost]
        public IActionResult AddRoom([FromBody] Room room)
        {
            _context.Rooms.Add(room);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetRoomById), new { id = room.Id }, room);
        }

        // Update a room
        [HttpPut("{id}")]
        public IActionResult UpdateRoom(int id, [FromBody] Room updatedRoom)
        {
            var room = _context.Rooms.Find(id);
            if (room == null)
            {
                return NotFound("Room not found.");
            }

            room.RoomNumber = updatedRoom.RoomNumber;
            room.Capacity = updatedRoom.Capacity;
            room.PricePerNight = updatedRoom.PricePerNight;
            room.IsAvailable = updatedRoom.IsAvailable;
            room.Description = updatedRoom.Description;

            _context.Entry(room).State = EntityState.Modified;
            _context.SaveChanges();
            return Ok(room);
        }

        // Delete a room
        [HttpDelete("{id}")]
        public IActionResult DeleteRoom(int id)
        {
            var room = _context.Rooms.Find(id);
            if (room == null)
            {
                return NotFound("Room not found.");
            }

            _context.Rooms.Remove(room);
            _context.SaveChanges();
            return Ok("Room deleted successfully.");
        }
 

        // Check room availability for specific dates
        [HttpGet("check-availability")]
        public IActionResult CheckAvailability([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            if (startDate >= endDate)
            {
                return BadRequest("Invalid date range: startDate must be earlier than endDate.");
            }

            // Get rooms that are available in the specified date range
            var availableRooms = _context.Rooms
                .Where(room => room.IsAvailable) // Only consider rooms marked as available

                .ToList();

            return Ok(availableRooms);
        }
    }
}

