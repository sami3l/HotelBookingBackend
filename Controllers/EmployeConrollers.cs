using Backend.Data;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [Route("api/employees")]
    [ApiController]

    public class EmployeControllers : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public EmployeControllers(ApplicationDBContext context)
        {
            _context = context;
        }
        
        // Get all employees
        [HttpGet]
        public IActionResult GetAllEmployees()
        {
            var AllEmployees = _context.Employees.ToList();

            return Ok(AllEmployees);
        }

        // Get a single employee by ID
        [HttpGet("{id}")]
        public IActionResult GetEmployeesById([FromRoute]int id)
        {
            var OneEmploye = _context.Employees.Find(id);

            if (OneEmploye == null)
            {
                return NotFound();
            }

            return Ok(OneEmploye);
        }

        // Register a new employee
        [HttpPost("register")]
        public IActionResult RegisterEmployee([FromBody] Employee employee)
        {
            if (_context.Employees.Any(e => e.Email == employee.Email))
            {
                return BadRequest("Employee with this email already exists.");
            }

            _context.Employees.Add(employee);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetEmployeesById), new { id = employee.Id }, employee);
        }

        // Login employee
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestEmployee request)
        {
            var employee = _context.Employees.FirstOrDefault(e => e.Email == request.Email && e.Password == request.Password);
            if (employee == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            // For simplicity, return employee details (Token generation should be added in production)
            return Ok(new { Message = "Login successful", Employee = employee });
        }

        // Update employee profile
        [HttpPut("{id}/profile")]
        public IActionResult UpdateProfile([FromRoute] int id, [FromBody] Employee updatedEmployee)
        {
            var employee = _context.Employees.Find(id);
            if (employee == null)
            {
                return NotFound("Employee not found.");
            }

            employee.Username = updatedEmployee.Username;
            employee.Email = updatedEmployee.Email;
            employee.Password = updatedEmployee.Password;
            // Update other fields as necessary

            _context.Entry(employee).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(employee);
        }

                // Change password
        [HttpPut("{id}/password")]
        public IActionResult ChangePassword([FromRoute] int id, [FromBody] ChangePasswordRequestEmployee request)
        {
            var employee = _context.Employees.Find(id);
            if (employee == null)
            {
                return NotFound("Employee not found.");
            }

            if (employee.Password != request.CurrentPassword)
            {
                return BadRequest("Current password is incorrect.");
            }

            employee.Password = request.NewPassword;

            _context.Entry(employee).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok("Password updated successfully.");
        }
    }

    // Helper Models for Login and Password Change
    public class LoginRequestEmployee
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class ChangePasswordRequestEmployee
    {
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }

    }
