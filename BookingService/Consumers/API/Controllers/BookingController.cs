using Application;
using Application.Booking.DTO;
using Application.Booking.Ports;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingManager _bookingManager;
        private readonly ILogger<BookingController> _logger;

        public BookingController(IBookingManager bookingManager, 
                                 ILogger<BookingController> logger)
        {
            _bookingManager = bookingManager;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<BookingDto>> Post(BookingDto booking)
        {
            var res = await _bookingManager.CreateBooking(booking);

            if (res.Success) return Created("", res.Data);

            else if (res.ErrorCode == ErrorCodes.BOOKING_MISSING_REQUIRED_INFORMATION)
            {
                return BadRequest(res);
            }
            else if (res.ErrorCode == ErrorCodes.BOOKING_DATE_INVALID_INFORMATION)
            {
                return BadRequest(res);
            }
            else if (res.ErrorCode == ErrorCodes.BOOKING_COULD_NOT_STORE_DATA)
            {
                return BadRequest(res);
            }

            _logger.LogError("Response with unknown ErrorCode returned", res);
            return BadRequest(500);
        }
    }
}
