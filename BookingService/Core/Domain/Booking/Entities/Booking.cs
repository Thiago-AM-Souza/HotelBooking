using Domain.Booking.Enums;
using Domain.Booking.Exceptions;
using Domain.Booking.Ports;
using Action = Domain.Booking.Enums.Action;

namespace Domain.Booking.Entities
{
    public class Booking
    {
        public Booking()
        {
            Status = Status.Created;
        }
        public int Id { get; set; }
        public DateTime PlacedAt { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public Status Status { get; set; }
        public Guest.Entities.Guest Guest { get; set; }
        public Room.Entities.Room Room { get; set; }

        public void ChangeState(Action action)
        {
            Status = (Status, action) switch
            {
                (Status.Created, Action.Pay) => Status.Paid,
                (Status.Created, Action.Cancel) => Status.Canceled,
                (Status.Paid, Action.Finish) => Status.Finished,
                (Status.Paid, Action.Refund) => Status.Refouded,
                (Status.Canceled, Action.Reopen) => Status.Created,
                _ => Status
            };
        }

        private void ValidateState()
        {
            if (PlacedAt == DateTime.MinValue)
            {
                throw new PlacedAtIsARequiredInformationException();
            }
            
            if (Start == DateTime.MinValue)
            {
                throw new StartDateTimeIsRequiredException();
            }
            
            if (End == DateTime.MinValue)
            {
                throw new EndDateTimeIsRequiredException();
            }

            if (Room == null)
            {
                throw new RoomIsRequiredException();
            }

            if (Guest == null)
            {
                throw new GuestIsRequiredException();
            }            
        }

        public async Task Save(IBookingRepository bookingRepository)
        {
            ValidateState();

            Guest.IsValid();

            if (!Room.CanBeBooked())
            {
                throw new RoomCannotBeBookedException();
            }

            if (Id == 0)
            {
                var resp = await bookingRepository.CreateBooking(this);

                Id = resp.Id;
            }
        }
    }
}
