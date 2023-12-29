using Application.Booking.DTO;
using Application.Booking.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Booking.Ports
{
    public interface IBookingManager
    {
        Task<BookingResponse> CreateBooking(BookingDto booking);
        Task<BookingDto> GetBooking(int id);
    }
}
