using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using pusgs_be.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pusgs_be.Infrastructure.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.Address)
                .IsRequired()
                .HasMaxLength(15);

            builder.Property(x => x.Comment)
                .HasMaxLength(30);

            builder.Property(x => x.Price)
                .IsRequired();

            builder.Property(x => x.Status).HasDefaultValue(OrderStatus.Pending);

            builder.HasOne(x => x.Deliverer)
                .WithOne(x => x.Order)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
