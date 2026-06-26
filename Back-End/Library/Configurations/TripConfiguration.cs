using Library.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Configurations
{
    public class TripConfiguration : IEntityTypeConfiguration<Trip>
    {
        public void Configure(EntityTypeBuilder<Trip> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(t => t.Description)
                .HasMaxLength(1000);

            builder.Property(t => t.StartLocation)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(t => t.Price)
                .HasColumnType("decimal(18,2)");

            builder.HasOne(t => t.Captain)
                .WithMany(c => c.Trips)
                .HasForeignKey(t => t.CaptainId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.Boat)
                .WithMany(b => b.Trips)
                .HasForeignKey(t => t.BoatId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}