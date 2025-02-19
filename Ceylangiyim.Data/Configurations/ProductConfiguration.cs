﻿using Ceylangiyim.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ceylangiyim.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasMaxLength(250);
            builder.Property(x => x.Image).IsRequired().HasMaxLength(100);
            builder.Property(x => x.ProductCode).HasMaxLength(100);

        }
    }
}
