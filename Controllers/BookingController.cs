using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelEase.Service.Interface;
using TravelEase.Service.Implementation;
using TravelEase.Constants;
namespace TravelEase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        public BookingController(IBookingService bookingService)
        {

            _bookingService=bookingService;

        }

        [HttpGet]
        [Route("AllBookings")]
        public IActionResult GetAllBookings()
        {
            var bookings = _bookingService.GetAllBookings();
            return Ok(bookings);
        }

        [HttpGet]
        [Route("bookingId")]
        public IActionResult GetBooking(int bookingId)
        {
            if (bookingId > 0)
            {
                var bookings = _bookingService.GetBooking(bookingId);
                return Ok(bookings);
            }
            return BadRequest(Constants.Constants.BadRequestMessage);
        
        }
    }

}
