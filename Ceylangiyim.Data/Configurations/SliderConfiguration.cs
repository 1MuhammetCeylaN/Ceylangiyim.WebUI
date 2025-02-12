using Ceylangiyim.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ceylangiyim.Data.Configurations
{
    public class SliderConfiguration : IEntityTypeConfiguration<Slider>
    {
        public void Configure(EntityTypeBuilder<Slider> builder)
        {
            builder.Property(x => x.Title).IsRequired().HasMaxLength(750);
            builder.Property(x => x.Description);
            builder.Property(x => x.Image).IsRequired().HasMaxLength(150);
            builder.Property(x => x.Link).HasMaxLength(150);
        }
    }
}
