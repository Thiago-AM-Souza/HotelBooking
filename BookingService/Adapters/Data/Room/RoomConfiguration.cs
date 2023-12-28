using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;


namespace Data.Room
{
    public class RoomConfiguration : IEntityTypeConfiguration<Domain.Room.Entities.Room>
    {
        public void Configure(EntityTypeBuilder<Domain.Room.Entities.Room> builder)
        {
            builder.HasKey(x => x.Id);
            builder.OwnsOne(x => x.Price)
                .Property(x => x.Value);

            builder.OwnsOne(x => x.Price)
                .Property(x => x.Currency);
        }
    }
}