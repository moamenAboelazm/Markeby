using Library.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Configurations
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(15);

            builder.Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(15);

            builder.HasMany(u => u.Trips)
                .WithMany(t => t.Passengers)
                .UsingEntity(j => j.ToTable("TripPassengers"));
        }
    }
}
