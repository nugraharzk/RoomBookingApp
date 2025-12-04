using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoomBookingAPI.Data;
using RoomBookingAPI.DTOs;
using RoomBookingAPI.Models;
using System.Security.Claims;
using Swashbuckle.AspNetCore.Annotations;

namespace RoomBookingAPI.Controllers
{
    /// <summary>
    /// Manages room bookings and reservations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    [Produces("application/json")]
    public class BookingsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all bookings (Admin sees all, Users see only their own)
        /// </summary>
        /// <returns>List of bookings</returns>
        /// <response code="200">Returns the list of bookings</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet]
        [SwaggerOperation(
            Summary = "Get all bookings",
            Description = "Retrieves bookings. Admins see all bookings, regular users see only their own.",
            Tags = new[] { "Bookings" }
        )]
        [ProducesResponseType(typeof(IEnumerable<BookingDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<BookingDto>>> GetBookings()
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            IQueryable<Booking> query = _context.Bookings
                .Include(b => b.Room)
                .Include(b => b.User);

            // If not admin, only show user's own bookings
            if (userRole != "Admin")
            {
                query = query.Where(b => b.UserId == userId);
            }

            var bookings = await query
                .Select(b => new BookingDto
                {
                    Id = b.Id,
                    RoomId = b.RoomId,
                    UserId = b.UserId,
                    StartTime = b.StartTime,
                    EndTime = b.EndTime,
                    Purpose = b.Purpose,
                    Status = b.Status,
                    CreatedAt = b.CreatedAt,
                    UpdatedAt = b.UpdatedAt,
                    Room = new RoomDto
                    {
                        Id = b.Room.Id,
                        Name = b.Room.Name,
                        Description = b.Room.Description,
                        Capacity = b.Room.Capacity,
                        Location = b.Room.Location,
                        IsAvailable = b.Room.IsAvailable,
                        PricePerHour = b.Room.PricePerHour
                    },
                    User = new UserDto
                    {
                        Id = b.User.Id,
                        Username = b.User.Username,
                        Email = b.User.Email,
                        Role = b.User.Role
                    }
                })
                .ToListAsync();

            return Ok(bookings);
        }

        /// <summary>
        /// Get a specific booking by ID
        /// </summary>
        /// <param name="id">Booking ID</param>
        /// <returns>Booking details</returns>
        /// <response code="200">Returns the booking details</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden - Cannot access other user's bookings</response>
        /// <response code="404">Booking not found</response>
        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Get booking by ID",
            Description = "Retrieves detailed information about a specific booking. Users can only access their own bookings.",
            Tags = new[] { "Bookings" }
        )]
        [ProducesResponseType(typeof(BookingDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookingDto>> GetBooking(int id)
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            var booking = await _context.Bookings
                .Include(b => b.Room)
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null)
            {
                return NotFound(new { message = "Booking not found" });
            }

            // Check authorization
            if (userRole != "Admin" && booking.UserId != userId)
            {
                return Forbid();
            }

            var bookingDto = new BookingDto
            {
                Id = booking.Id,
                RoomId = booking.RoomId,
                UserId = booking.UserId,
                StartTime = booking.StartTime,
                EndTime = booking.EndTime,
                Purpose = booking.Purpose,
                Status = booking.Status,
                CreatedAt = booking.CreatedAt,
                UpdatedAt = booking.UpdatedAt,
                Room = new RoomDto
                {
                    Id = booking.Room.Id,
                    Name = booking.Room.Name,
                    Description = booking.Room.Description,
                    Capacity = booking.Room.Capacity,
                    Location = booking.Room.Location,
                    IsAvailable = booking.Room.IsAvailable,
                    PricePerHour = booking.Room.PricePerHour
                },
                User = new UserDto
                {
                    Id = booking.User.Id,
                    Username = booking.User.Username,
                    Email = booking.User.Email,
                    Role = booking.User.Role
                }
            };

            return Ok(bookingDto);
        }

        /// <summary>
        /// Create a new booking
        /// </summary>
        /// <param name="dto">Booking details</param>
        /// <returns>Created booking</returns>
        /// <response code="201">Booking successfully created</response>
        /// <response code="400">Invalid input or room already booked for this time slot</response>
        /// <response code="401">Unauthorized</response>
        [HttpPost]
        [SwaggerOperation(
            Summary = "Create a new booking",
            Description = "Creates a new room booking. Validates time slots and checks for conflicts.",
            Tags = new[] { "Bookings" }
        )]
        [ProducesResponseType(typeof(BookingDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<BookingDto>> CreateBooking(CreateBookingDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            // Validate room exists
            var room = await _context.Rooms.FindAsync(dto.RoomId);
            if (room == null)
            {
                return BadRequest(new { message = "Room not found" });
            }

            // Validate time range
            if (dto.StartTime >= dto.EndTime)
            {
                return BadRequest(new { message = "End time must be after start time" });
            }

            if (dto.StartTime < DateTime.UtcNow)
            {
                return BadRequest(new { message = "Cannot book in the past" });
            }

            // Check for conflicting bookings
            var hasConflict = await _context.Bookings
                .AnyAsync(b => b.RoomId == dto.RoomId &&
                              b.Status != "Cancelled" &&
                              ((dto.StartTime >= b.StartTime && dto.StartTime < b.EndTime) ||
                               (dto.EndTime > b.StartTime && dto.EndTime <= b.EndTime) ||
                               (dto.StartTime <= b.StartTime && dto.EndTime >= b.EndTime)));

            if (hasConflict)
            {
                return BadRequest(new { message = "Room is already booked for this time slot" });
            }

            var booking = new Booking
            {
                RoomId = dto.RoomId,
                UserId = userId,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                Purpose = dto.Purpose,
                Status = "Confirmed",
                CreatedAt = DateTime.UtcNow
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            // Load navigation properties
            await _context.Entry(booking).Reference(b => b.Room).LoadAsync();
            await _context.Entry(booking).Reference(b => b.User).LoadAsync();

            var bookingDto = new BookingDto
            {
                Id = booking.Id,
                RoomId = booking.RoomId,
                UserId = booking.UserId,
                StartTime = booking.StartTime,
                EndTime = booking.EndTime,
                Purpose = booking.Purpose,
                Status = booking.Status,
                CreatedAt = booking.CreatedAt,
                Room = new RoomDto
                {
                    Id = booking.Room.Id,
                    Name = booking.Room.Name,
                    Description = booking.Room.Description,
                    Capacity = booking.Room.Capacity,
                    Location = booking.Room.Location,
                    IsAvailable = booking.Room.IsAvailable,
                    PricePerHour = booking.Room.PricePerHour
                },
                User = new UserDto
                {
                    Id = booking.User.Id,
                    Username = booking.User.Username,
                    Email = booking.User.Email,
                    Role = booking.User.Role
                }
            };

            return CreatedAtAction(nameof(GetBooking), new { id = booking.Id }, bookingDto);
        }

        /// <summary>
        /// Update an existing booking
        /// </summary>
        /// <param name="id">Booking ID</param>
        /// <param name="dto">Updated booking details</param>
        /// <returns>No content</returns>
        /// <response code="204">Booking successfully updated</response>
        /// <response code="400">Invalid input</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden - Cannot update other user's bookings</response>
        /// <response code="404">Booking not found</response>
        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Update a booking",
            Description = "Updates an existing booking. Users can only update their own bookings.",
            Tags = new[] { "Bookings" }
        )]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateBooking(int id, UpdateBookingDto dto)
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            var booking = await _context.Bookings.FindAsync(id);

            if (booking == null)
            {
                return NotFound(new { message = "Booking not found" });
            }

            // Check authorization
            if (userRole != "Admin" && booking.UserId != userId)
            {
                return Forbid();
            }

            // Update fields
            if (dto.StartTime.HasValue) booking.StartTime = dto.StartTime.Value;
            if (dto.EndTime.HasValue) booking.EndTime = dto.EndTime.Value;
            if (dto.Purpose != null) booking.Purpose = dto.Purpose;
            if (dto.Status != null) booking.Status = dto.Status;

            booking.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await BookingExists(id))
                {
                    return NotFound(new { message = "Booking not found" });
                }
                throw;
            }

            return NoContent();
        }

        /// <summary>
        /// Delete a booking
        /// </summary>
        /// <param name="id">Booking ID</param>
        /// <returns>No content</returns>
        /// <response code="204">Booking successfully deleted</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden - Cannot delete other user's bookings</response>
        /// <response code="404">Booking not found</response>
        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Delete a booking",
            Description = "Deletes an existing booking. Users can only delete their own bookings.",
            Tags = new[] { "Bookings" }
        )]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            var booking = await _context.Bookings.FindAsync(id);

            if (booking == null)
            {
                return NotFound(new { message = "Booking not found" });
            }

            // Check authorization
            if (userRole != "Admin" && booking.UserId != userId)
            {
                return Forbid();
            }

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> BookingExists(int id)
        {
            return await _context.Bookings.AnyAsync(e => e.Id == id);
        }
    }
}
