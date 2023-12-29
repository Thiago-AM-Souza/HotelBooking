using Domain.Booking.Ports;
using Microsoft.EntityFrameworkCore;

namespace Data.Booking
{
    public class BookingRepository : IBookingRepository
    {
        private readonly HotelDbContext _dbContext;

        public BookingRepository(HotelDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task<Domain.Booking.Entities.Booking> CreateBooking(Domain.Booking.Entities.Booking booking)
        {
            _dbContext.Bookings.Add(booking);
            await _dbContext.SaveChangesAsync();
            return booking;
        }

        public Task<Domain.Booking.Entities.Booking> Get(int id)
        {
            return _dbContext.Bookings.Where(x => x.Id == id).FirstAsync();
        }
    }
}
