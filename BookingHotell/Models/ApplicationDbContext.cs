using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingHotell.Models
{
    public class ApplicationDbContext : DbContext
    {

        public DbSet<Room> Rooms { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Room>().HasData(
                new Room
                {
                    RoomId = 1,  
                    RoomType = RoomType.Single,
                    Capacity = 1,
                    PricePerNight = 500,
                    HasExtraBedOption = false,
                    ExtraBedsAvailable = 0
                },
                new Room
                {
                    RoomId = 2, 
                    RoomType = RoomType.Double,
                    Capacity = 2,
                    PricePerNight = 1000,
                    HasExtraBedOption = true,
                    ExtraBedsAvailable = 2
                },
                new Room
                {
                    RoomId = 3,  
                    RoomType = RoomType.Double,
                    Capacity = 2,
                    PricePerNight = 1200,
                    HasExtraBedOption = true,
                    ExtraBedsAvailable = 2
                },
                new Room
                {
                    RoomId = 4,  
                    RoomType = RoomType.Single,
                    Capacity = 1,
                    PricePerNight = 450,
                    HasExtraBedOption = false,
                    ExtraBedsAvailable = 0
                }
            );

            modelBuilder.Entity<Customer>().HasData(
                new Customer { CustomerId = 1, FirstName = "Alice", LastName = "Smith", PhoneNumber = "123456789", Email = "alice@example.com" },
                new Customer { CustomerId = 2, FirstName = "Bob", LastName = "Johnson", PhoneNumber = "987654321", Email = "bob@example.com" },
                new Customer { CustomerId = 3, FirstName = "Charlie", LastName = "Brown", PhoneNumber = "456789123", Email = "charlie@example.com" },
                new Customer { CustomerId = 4, FirstName = "Diana", LastName = "Jones", PhoneNumber = "789123456", Email = "diana@example.com" }
            );
        }

    }

}







