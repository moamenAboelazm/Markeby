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

            builder.Property(c => c.FullName).IsRequired().HasMaxLength(150);
            builder.Property(c => c.Email).IsRequired().HasMaxLength(100);
            builder.Property(c => c.PhoneNumber).IsRequired().HasMaxLength(20);
            builder.Property(c => c.Rank).HasMaxLength(100);
            builder.Property(c => c.Languages).HasMaxLength(200);
            builder.Property(c => c.Bio).HasMaxLength(1000);
        }
    }
}
