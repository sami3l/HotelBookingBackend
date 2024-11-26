using Backend.Data;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Backend.Controllers
{
    [Route("api/clients")]
    [ApiController]
    public class ClientControllers : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public ClientControllers(ApplicationDBContext context)
        {
            _context = context;
        }

        // Get all clients
        [HttpGet]
        public IActionResult GetAllClients()
        {
            var AllClients = _context.Clients.ToList();
            return Ok(AllClients);
        }

        // Get a single client by ID
        [HttpGet("{id}")]
        public IActionResult GetClientById([FromRoute] int id)
        {
            var OneClient = _context.Clients.Find(id);
            if (OneClient == null)
            {
                return NotFound();
            }
            return Ok(OneClient);
        }

        // Register a new client
        [HttpPost("register")]
        public IActionResult RegisterClient([FromBody] Client client)
        {
            if (_context.Clients.Any(c => c.Email == client.Email))
            {
                return BadRequest("Client with this email already exists.");
            }

            _context.Clients.Add(client);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetClientById), new { id = client.Id }, client);
        }

        // Login client
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestClient request)
        {
            var client = _context.Clients.FirstOrDefault(c => c.Email == request.Email && c.Password == request.Password);
            if (client == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            // For simplicity, return client details (Token generation should be added in production)
            return Ok(new { Message = "Login successful", Client = client });
        }

        // Update client profile
        [HttpPut("{id}/profile")]
        public IActionResult UpdateProfile([FromRoute] int id, [FromBody] Client updatedClient)
        {
            var client = _context.Clients.Find(id);
            if (client == null)
            {
                return NotFound("Client not found.");
            }

            client.Username = updatedClient.Username;
            client.Email = updatedClient.Email;
            client.Password = updatedClient.Password;
            // Update other fields as necessary

            _context.Entry(client).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(client);
        }

        // Change password
        [HttpPut("{id}/password")]
        public IActionResult ChangePassword([FromRoute] int id, [FromBody] ChangePasswordRequestClient request)
        {
            var client = _context.Clients.Find(id);
            if (client == null)
            {
                return NotFound("Client not found.");
            }

            if (client.Password != request.CurrentPassword)
            {
                return BadRequest("Current password is incorrect.");
            }

            client.Password = request.NewPassword;

            _context.Entry(client).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok("Password updated successfully.");
        }

        // Delete a client
        [HttpDelete("{id}")]
        public IActionResult DeleteClient(int id)

        {
            var client = _context.Clients.Find(id);
            if (client == null)
            {
                return NotFound("Client not found.");
            }

            _context.Clients.Remove(client);
            _context.SaveChanges();
            return Ok("Client deleted successfully.");
        }

        // Delete all clients
        [HttpDelete("deleteall")]
        public IActionResult DeleteAllClients()
        {
            var allClients = _context.Clients.ToList();
            if (!allClients.Any())
            {
                return NotFound("No clients found.");
            }

            _context.Clients.RemoveRange(allClients);
            _context.SaveChanges();
            return Ok("All clients deleted successfully.");
        }
    }

    // Helper Models for Login and Password Change
    public class LoginRequestClient
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class ChangePasswordRequestClient
    {
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
