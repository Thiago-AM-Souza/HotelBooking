using Domain.Booking.Enums;
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
        private Status Status { get; set; }
        public Status CurrentStatus { get { return Status; } }
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
    }
}
