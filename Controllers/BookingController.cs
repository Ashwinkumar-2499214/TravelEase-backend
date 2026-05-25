using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelEase.Service.Interface;
using TravelEase.Service.Implementation;
using TravelEase.Constants;
using TravelEase.DTOs;

namespace TravelEase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        /// <summary>
        /// Creates a brand-new booking (initiating checkout).
        /// POST /api/bookings
        /// </summary>
        /// <param name="request">Booking creation request with optional reservations</param>
        /// <returns>Created booking with assigned IDs and timestamps</returns>
        [HttpPost]
        [Route("")]
        [ProducesResponseType(typeof(BookingResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateBooking([FromBody] CreateBookingRequest request)
        {
            if (request == null)
                return BadRequest(new { message = Constants.Constants.BadRequestMessage });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = _bookingService.CreateBooking(request);
            return CreatedAtAction(nameof(GetBooking), new { bookingId = created.BookingID }, created);
        }

        /// <summary>
        /// Retrieves all bookings.
        /// GET /api/bookings
        /// </summary>
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(IEnumerable<BookingResponse>), StatusCodes.Status200OK)]
        public IActionResult GetAllBookings()
        {
            var bookings = _bookingService.GetAllBookings();
            return Ok(bookings);
        }

        /// <summary>
        /// Retrieves a specific booking by ID with associated reservations.
        /// GET /api/bookings/{bookingId}
        /// </summary>
        [HttpGet]
        [Route("{bookingId}")]
        [ProducesResponseType(typeof(BookingResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetBooking(int bookingId)
        {
            if (bookingId <= 0)
                return BadRequest(new { message = Constants.Constants.BadRequestMessage });

            var booking = _bookingService.GetBooking(bookingId);
            if (booking == null)
                return NotFound(new { message = $"Booking with ID {bookingId} not found." });

            return Ok(booking);
        }

        /// <summary>
        /// Searches bookings by customer ID (booking history retrieval).
        /// GET /api/bookings/customer/{customerId}
        /// </summary>
        [HttpGet]
        [Route("customer/{customerId}")]
        [ProducesResponseType(typeof(IEnumerable<BookingResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetBookingsByCustomer(int customerId)
        {
            if (customerId <= 0)
                return BadRequest(new { message = Constants.Constants.BadRequestMessage });

            var bookings = _bookingService.SearchBookingsByCustomer(customerId);
            return Ok(bookings);
        }

        /// <summary>
        /// Searches bookings by status.
        /// GET /api/bookings/status/{status}
        /// </summary>
        [HttpGet]
        [Route("status/{status}")]
        [ProducesResponseType(typeof(IEnumerable<BookingResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetBookingsByStatus(string status)
        {
            if (string.IsNullOrWhiteSpace(status))
                return BadRequest(new { message = Constants.Constants.BadRequestMessage });

            var bookings = _bookingService.SearchBookingsByStatus(status);
            return Ok(bookings);
        }

        /// <summary>
        /// Modifies an existing booking.
        /// PUT /api/bookings/{bookingId}
        /// </summary>
        [HttpPut]
        [Route("{bookingId}")]
        [ProducesResponseType(typeof(BookingResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ModifyBooking(int bookingId, [FromBody] CreateBookingRequest request)
        {
            if (bookingId <= 0 || request == null)
                return BadRequest(new { message = Constants.Constants.BadRequestMessage });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var modified = _bookingService.ModifyBooking(bookingId, request);
            if (modified == null)
                return NotFound(new { message = $"Booking with ID {bookingId} not found." });

            return Ok(modified);
        }

        /// <summary>
        /// Cancels a booking and all associated reservations.
        /// DELETE /api/bookings/{bookingId}
        /// </summary>
        [HttpDelete]
        [Route("{bookingId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CancelBooking(int bookingId)
        {
            if (bookingId <= 0)
                return BadRequest(new { message = Constants.Constants.BadRequestMessage });

            var result = _bookingService.CancelBooking(bookingId);
            if (!result)
                return NotFound(new { message = $"Booking with ID {bookingId} not found." });

            return NoContent();
        }
    }
}
