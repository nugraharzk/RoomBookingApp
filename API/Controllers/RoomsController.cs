using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoomBookingAPI.Data;
using RoomBookingAPI.DTOs;
using RoomBookingAPI.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace RoomBookingAPI.Controllers
{
    /// <summary>
    /// Manages room resources and availability
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    [Produces("application/json")]
    public class RoomsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RoomsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all available rooms
        /// </summary>
        /// <returns>List of all rooms</returns>
        /// <response code="200">Returns the list of rooms</response>
        [HttpGet]
        [AllowAnonymous]
        [SwaggerOperation(
            Summary = "Get all rooms",
            Description = "Retrieves a list of all available rooms with their details",
            Tags = new[] { "Rooms" }
        )]
        [ProducesResponseType(typeof(IEnumerable<RoomDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<RoomDto>>> GetRooms()
        {
            var rooms = await _context.Rooms
                .Select(r => new RoomDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    Capacity = r.Capacity,
                    Location = r.Location,
                    IsAvailable = r.IsAvailable,

                    CreatedAt = r.CreatedAt,
                    UpdatedAt = r.UpdatedAt
                })
                .ToListAsync();

            return Ok(rooms);
        }

        /// <summary>
        /// Get a specific room by ID
        /// </summary>
        /// <param name="id">Room ID</param>
        /// <returns>Room details</returns>
        /// <response code="200">Returns the room details</response>
        /// <response code="404">Room not found</response>
        [HttpGet("{id}")]
        [AllowAnonymous]
        [SwaggerOperation(
            Summary = "Get room by ID",
            Description = "Retrieves detailed information about a specific room",
            Tags = new[] { "Rooms" }
        )]
        [ProducesResponseType(typeof(RoomDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RoomDto>> GetRoom(int id)
        {
            var room = await _context.Rooms.FindAsync(id);

            if (room == null)
            {
                return NotFound(new { message = "Room not found" });
            }

            var roomDto = new RoomDto
            {
                Id = room.Id,
                Name = room.Name,
                Description = room.Description,
                Capacity = room.Capacity,
                Location = room.Location,
                IsAvailable = room.IsAvailable,

                CreatedAt = room.CreatedAt,
                UpdatedAt = room.UpdatedAt
            };

            return Ok(roomDto);
        }

        /// <summary>
        /// Create a new room (Admin only)
        /// </summary>
        /// <param name="dto">Room details</param>
        /// <returns>Created room</returns>
        /// <response code="201">Room successfully created</response>
        /// <response code="400">Invalid input</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden - Admin role required</response>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(
            Summary = "Create a new room",
            Description = "Creates a new room resource. Requires Admin role.",
            Tags = new[] { "Rooms" }
        )]
        [ProducesResponseType(typeof(RoomDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<RoomDto>> CreateRoom(CreateRoomDto dto)
        {
            var room = new Room
            {
                Name = dto.Name,
                Description = dto.Description,
                Capacity = dto.Capacity,
                Location = dto.Location,
                IsAvailable = dto.IsAvailable,

                CreatedAt = DateTime.UtcNow
            };

            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();

            var roomDto = new RoomDto
            {
                Id = room.Id,
                Name = room.Name,
                Description = room.Description,
                Capacity = room.Capacity,
                Location = room.Location,
                IsAvailable = room.IsAvailable,

                CreatedAt = room.CreatedAt,
                UpdatedAt = room.UpdatedAt
            };

            return CreatedAtAction(nameof(GetRoom), new { id = room.Id }, roomDto);
        }

        /// <summary>
        /// Update an existing room (Admin only)
        /// </summary>
        /// <param name="id">Room ID</param>
        /// <param name="dto">Updated room details</param>
        /// <returns>No content</returns>
        /// <response code="204">Room successfully updated</response>
        /// <response code="400">Invalid input</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden - Admin role required</response>
        /// <response code="404">Room not found</response>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(
            Summary = "Update a room",
            Description = "Updates an existing room's details. Requires Admin role.",
            Tags = new[] { "Rooms" }
        )]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateRoom(int id, UpdateRoomDto dto)
        {
            var room = await _context.Rooms.FindAsync(id);

            if (room == null)
            {
                return NotFound(new { message = "Room not found" });
            }

            // Update only provided fields
            if (dto.Name != null) room.Name = dto.Name;
            if (dto.Description != null) room.Description = dto.Description;
            if (dto.Capacity.HasValue) room.Capacity = dto.Capacity.Value;
            if (dto.Location != null) room.Location = dto.Location;
            if (dto.IsAvailable.HasValue) room.IsAvailable = dto.IsAvailable.Value;


            room.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await RoomExists(id))
                {
                    return NotFound(new { message = "Room not found" });
                }
                throw;
            }

            return NoContent();
        }

        /// <summary>
        /// Delete a room (Admin only)
        /// </summary>
        /// <param name="id">Room ID</param>
        /// <returns>No content</returns>
        /// <response code="204">Room successfully deleted</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden - Admin role required</response>
        /// <response code="404">Room not found</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(
            Summary = "Delete a room",
            Description = "Deletes an existing room. Requires Admin role.",
            Tags = new[] { "Rooms" }
        )]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var room = await _context.Rooms.FindAsync(id);

            if (room == null)
            {
                return NotFound(new { message = "Room not found" });
            }

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> RoomExists(int id)
        {
            return await _context.Rooms.AnyAsync(e => e.Id == id);
        }
    }
}
