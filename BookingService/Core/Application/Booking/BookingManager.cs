using Application.Booking.DTO;
using Application.Booking.Ports;
using Application.Booking.Responses;
using Domain.Booking.Exceptions;
using Domain.Booking.Ports;
using Domain.Guest.Ports;
using Domain.Room.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Booking
{
    public class BookingManager : IBookingManager
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IGuestRepository _guestRepository;
        private readonly IRoomRepository _roomRepository;

        public BookingManager(IBookingRepository bookingRepository,
                              IGuestRepository guestRepository,
                              IRoomRepository roomRepository)
        {
            _bookingRepository = bookingRepository;
            _guestRepository = guestRepository;
            _roomRepository = roomRepository;
        }

        public async Task<BookingResponse> CreateBooking(BookingDto bookingDto)
        {
            try
            {
                var booking = BookingDto.MapToEntity(bookingDto);

                booking.Guest = await _guestRepository.Get(bookingDto.GuestId);
                booking.Room = await _roomRepository.GetAggregate(bookingDto.RoomId);

                await booking.Save(_bookingRepository);

                bookingDto.Id = bookingDto.Id;

                return new BookingResponse
                {
                    Success = true,
                    Data = bookingDto,
                };
            }
            catch (PlacedAtIsARequiredInformationException)
            {
                return new BookingResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.ROOM_MISSING_REQUIRED_INFORMATION,
                    Message = "PlacedAt is a required information."
                };
            }
            catch (EndDateTimeIsRequiredException)
            {
                return new BookingResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.BOOKING_DATE_INVALID_INFORMATION,
                    Message = "EndDateTime is a required information"
                };
            }
            catch (GuestIsRequiredException)
            {
                return new BookingResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.MISSING_REQUIRED_INFORMATION,
                    Message = "Guest is a required information"
                };
            }
            catch (RoomIsRequiredException)
            {
                return new BookingResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.ROOM_MISSING_REQUIRED_INFORMATION,
                    Message = "Room is a required information"
                };
            }
            catch (StartDateTimeIsRequiredException)
            {
                return new BookingResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.BOOKING_DATE_INVALID_INFORMATION,
                    Message = "StartDateTime is a required information"
                };
            }
            catch (Exception)
            {
                return new BookingResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.BOOKING_COULD_NOT_STORE_DATA,
                    Message = "There is an error in DB"
                };
            }
        }

        public Task<BookingDto> GetBooking(int id)
        {
            throw new NotImplementedException();
        }
    }
}
