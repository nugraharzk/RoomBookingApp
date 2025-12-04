using RoomBookingAPI.Models;
using BCrypt.Net;
using Microsoft.Extensions.Logging;

namespace RoomBookingAPI.Data
{
    public class DbSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context, ILogger logger)
        {
            // Seed Users
            if (!context.Users.Any())
            {
                logger.LogInformation("Seeding Users...");
                var users = new List<User>
                {
                    new User
                    {
                        Username = "admin",
                        Email = "admin@roombooking.com",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                        Role = "Admin",
                        CreatedAt = DateTime.UtcNow
                    },
                    new User
                    {
                        Username = "john_doe",
                        Email = "john@example.com",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("User123!"),
                        Role = "User",
                        CreatedAt = DateTime.UtcNow
                    },
                    new User
                    {
                        Username = "jane_smith",
                        Email = "jane@example.com",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("User123!"),
                        Role = "User",
                        CreatedAt = DateTime.UtcNow
                    }
                };

                await context.Users.AddRangeAsync(users);
                await context.SaveChangesAsync();
                logger.LogInformation("Users seeded successfully.");
            }
            else
            {
                logger.LogInformation("Users already exist. Skipping seed.");
            }

            // Seed Rooms
            if (!context.Rooms.Any())
            {
                logger.LogInformation("Seeding Rooms...");
                var rooms = new List<Room>
                {
                    new Room
                    {
                        Name = "Conference Room A",
                        Description = "Large conference room with projector and whiteboard",
                        Capacity = 20,
                        Location = "1st Floor, East Wing",
                        IsAvailable = true,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Room
                    {
                        Name = "Meeting Room B",
                        Description = "Small meeting room perfect for team discussions",
                        Capacity = 8,
                        Location = "2nd Floor, West Wing",
                        IsAvailable = true,

                        CreatedAt = DateTime.UtcNow
                    },
                    new Room
                    {
                        Name = "Board Room",
                        Description = "Executive board room with video conferencing",
                        Capacity = 15,
                        Location = "3rd Floor, Center",
                        IsAvailable = true,

                        CreatedAt = DateTime.UtcNow
                    },
                    new Room
                    {
                        Name = "Training Room",
                        Description = "Spacious training room with multiple workstations",
                        Capacity = 30,
                        Location = "1st Floor, West Wing",
                        IsAvailable = true,

                        CreatedAt = DateTime.UtcNow
                    },
                    new Room
                    {
                        Name = "Huddle Room",
                        Description = "Quick meeting space for small teams",
                        Capacity = 4,
                        Location = "2nd Floor, East Wing",
                        IsAvailable = true,

                        CreatedAt = DateTime.UtcNow
                    }
                };

                await context.Rooms.AddRangeAsync(rooms);
                await context.SaveChangesAsync();
                logger.LogInformation("Rooms seeded successfully.");
            }
            else
            {
                logger.LogInformation("Rooms already exist. Skipping seed.");
            }

            // Seed Bookings
            if (!context.Bookings.Any())
            {
                logger.LogInformation("Seeding Bookings...");
                var user = context.Users.FirstOrDefault(u => u.Username == "john_doe");
                var room = context.Rooms.FirstOrDefault(r => r.Name == "Conference Room A");

                if (user != null && room != null)
                {
                    var bookings = new List<Booking>
                    {
                        new Booking
                        {
                            RoomId = room.Id,
                            UserId = user.Id,
                            StartTime = DateTime.UtcNow.AddDays(1).AddHours(9),
                            EndTime = DateTime.UtcNow.AddDays(1).AddHours(11),
                            Purpose = "Team Planning Meeting",
                            Status = "Confirmed",
                            CreatedAt = DateTime.UtcNow
                        }
                    };

                    await context.Bookings.AddRangeAsync(bookings);
                    await context.SaveChangesAsync();
                    logger.LogInformation("Bookings seeded successfully.");
                }
                else
                {
                    logger.LogWarning("Could not seed bookings because user or room was not found.");
                }
            }
            else
            {
                logger.LogInformation("Bookings already exist. Skipping seed.");
            }
        }
    }
}
