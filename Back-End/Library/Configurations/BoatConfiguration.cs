using Library.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Configurations
{
    public class BoatConfiguration : IEntityTypeConfiguration<Boat>
    {
        public void Configure(EntityTypeBuilder<Boat> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasMany(b => b.Trips)
                .WithOne(t => t.Boat)
                .HasForeignKey(t => t.BoatId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(b => b.Images)
                .WithOne(i => i.Boat)
                .HasForeignKey(i => i.BoatId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
