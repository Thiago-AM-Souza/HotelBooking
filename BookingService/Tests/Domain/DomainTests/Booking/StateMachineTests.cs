using Domain.Booking.Enums;
using Action = Domain.Booking.Enums.Action;

namespace DomainTests.Bookings
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ShouldAlwaysStartWithCreatedStatus()
        {
            var booking = new Domain.Booking.Entities.Booking();

            Assert.That(Status.Created.Equals(booking.CurrentStatus));
        }

        [Test]
        public void ShouldSetStatusToPaidWhenPayingForABookingWithCreatedStatus()
        {
            var booking = new Domain.Booking.Entities.Booking();
            booking.ChangeState(Action.Pay);

            Assert.That(Status.Paid.Equals(booking.CurrentStatus));
        }

        [Test]
        public void ShouldSetStatusToCanceledWhenCancelABookingWithCreatedStatus()
        {
            var booking = new Domain.Booking.Entities.Booking();
            booking.ChangeState(Action.Cancel);

            Assert.That(Status.Canceled.Equals(booking.CurrentStatus));
        }

        [Test]
        public void ShouldSetStatusToFinishedWhenPayingForABookingWithPaidBooking()
        {
            var booking = new Domain.Booking.Entities.Booking();
            booking.ChangeState(Action.Pay);
            booking.ChangeState(Action.Finish);

            Assert.That(Status.Finished.Equals(booking.CurrentStatus));
        }

        [Test]
        public void ShouldSetStatusToRefoundedWhenPayingForABookingWithPaidBooking()
        {
            var booking = new Domain.Booking.Entities.Booking();
            booking.ChangeState(Action.Pay);
            booking.ChangeState(Action.Refund);

            Assert.That(Status.Refouded.Equals(booking.CurrentStatus));
        }

        [Test]
        public void ShouldSetStatusToCreatedWhenACanceledBooking()
        {
            var booking = new Domain.Booking.Entities.Booking();
            booking.ChangeState(Action.Cancel);
            booking.ChangeState(Action.Reopen);

            Assert.That(Status.Created.Equals(booking.CurrentStatus));
        }
    }
}