using Application.Booking.DTO;
using Application.Booking.Ports;
using Application.Booking.Responses;
using Application.Payment.Ports;
using Domain.Booking.Exceptions;
using Domain.Booking.Ports;
using Domain.Guest.Ports;
using Domain.Room.Ports;
using MediatR;

namespace Application.Booking.Commands
{
    public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, BookingResponse>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IGuestRepository _guestRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IPaymentProcessorFactory _paymentProcessorFactory;

        public CreateBookingCommandHandler(IBookingRepository bookingRepository,
                                            IGuestRepository guestRepository,
                                            IRoomRepository roomRepository,
                                            IPaymentProcessorFactory paymentProcessorFactory)
        {
            _bookingRepository = bookingRepository;
            _guestRepository = guestRepository;
            _roomRepository = roomRepository;
            _paymentProcessorFactory = paymentProcessorFactory;
        }

        public async Task<BookingResponse> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var booking = BookingDto.MapToEntity(request.BookingDto);

                booking.Guest = await _guestRepository.Get(request.BookingDto.GuestId);
                booking.Room = await _roomRepository.GetAggregate(request.BookingDto.RoomId);
                await booking.Save(_bookingRepository);

                request.BookingDto.Id = request.BookingDto.Id;

                return new BookingResponse
                {
                    Success = true,
                    Data = request.BookingDto,
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
    }
}
