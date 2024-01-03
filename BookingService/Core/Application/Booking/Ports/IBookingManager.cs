using Application.Booking.DTO;
using Application.Booking.Dtos;
using Application.Booking.Responses;
using Application.Payment.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Booking.Ports
{
    public interface IBookingManager
    {
        Task<BookingDto> GetBooking(int id);
        Task<PaymentResponse> PayForABooking(PaymentRequestDto paymentRequestDto);
    }
}
