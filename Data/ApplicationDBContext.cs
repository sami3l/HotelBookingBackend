using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
        }
        public DbSet<Models.Client> Clients { get; set; }
        public DbSet<Models.Employee> Employees { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Booking> Bookings { get; set; }


    }
}