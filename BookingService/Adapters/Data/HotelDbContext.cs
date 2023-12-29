using Data.Guest;
using Data.Room;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class HotelDbContext : DbContext
    {
        public HotelDbContext(DbContextOptions options) : base(options) { }

        public virtual DbSet<Domain.Booking.Entities.Booking> Bookings { get; set; }
        public virtual DbSet<Domain.Guest.Entities.Guest> Guests { get; set; }
        public virtual DbSet<Domain.Room.Entities.Room> Rooms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new GuestConfiguration());
            modelBuilder.ApplyConfiguration(new RoomConfiguration());
        }
    }
}