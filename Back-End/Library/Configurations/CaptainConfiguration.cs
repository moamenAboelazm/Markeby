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
    public class CaptainConfiguration : IEntityTypeConfiguration<Captain>
    {
        public void Configure(EntityTypeBuilder<Captain> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasMany(c => c.Boats)
                .WithMany(b => b.Captains)
                .UsingEntity(j => j.ToTable("BoatCaptains"));
        }
    }
}
