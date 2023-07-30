using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using pusgs_be.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pusgs_be.Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.Username)
                .IsRequired()
                .HasMaxLength(15);
            builder.HasIndex(x => x.Username).IsUnique();

            builder.Property(x => x.Password)
                .IsRequired()
                .HasColumnName("PasswordHash");

            builder.Property(x => x.Email).IsRequired();

            builder.Property(x => x.FirstName)
                .IsRequired()
                .HasMaxLength(15);
            builder.Property(x => x.LastName)
                .IsRequired()
                .HasMaxLength(15);

            builder.Property(x => x.BirthDate).IsRequired();

            builder.Property(x => x.Type).IsRequired();

            builder.Property(x => x.IsApproved).HasDefaultValue(UserApprove.Processing);
        }
    }
}
