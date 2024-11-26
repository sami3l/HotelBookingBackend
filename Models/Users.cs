namespace Backend.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class Client : User
    {
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }

    public class Employee : User
    {
        public string Role { get; set; } = string.Empty;
        public DateTime HireDate { get; set; }
    }
}
