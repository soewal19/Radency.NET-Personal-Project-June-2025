using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeskBooking.Application.Services;
using DeskBooking.Core.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DeskBooking.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly ILogger<BookingsController> _logger;

        public BookingsController(
            IBookingService bookingService,
            ILogger<BookingsController> logger)
        {
            _bookingService = bookingService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<BookingDto>> CreateBooking(CreateBookingDto dto)
        {
            try
            {
                var booking = await _bookingService.CreateBookingAsync(dto);
                return CreatedAtAction(nameof(GetBooking), new { id = booking.Id }, booking);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating booking");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookingDto>> GetBooking(Guid id)
        {
            try
            {
                var booking = await _bookingService.GetBookingAsync(id);
                if (booking == null)
                {
                    return NotFound();
                }
                return Ok(booking);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting booking {BookingId}", id);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookingDto>>> GetUserBookings([FromQuery] string userEmail)
        {
            try
            {
                var bookings = await _bookingService.GetUserBookingsAsync(userEmail);
                return Ok(bookings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting bookings for user {UserEmail}", userEmail);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<BookingDto>> UpdateBooking(Guid id, UpdateBookingDto dto)
        {
            try
            {
                var booking = await _bookingService.UpdateBookingAsync(id, dto);
                return Ok(booking);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Booking {BookingId} not found", id);
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating booking {BookingId}", id);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBooking(Guid id)
        {
            try
            {
                await _bookingService.DeleteBookingAsync(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Booking {BookingId} not found", id);
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting booking {BookingId}", id);
                return BadRequest(new { message = ex.Message });
            }
        }
    }
} 