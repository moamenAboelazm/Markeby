using Library.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }
    }
}
