using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Configuration
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("orders");

            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id)
                   .ValueGeneratedOnAdd()
                   .IsRequired()
                   .HasColumnName("id");

            builder.Property(o => o.UserId)
                   .IsRequired()
                   .HasColumnName("user_id");

            builder.Property(o => o.Total)
                .IsRequired()
                .HasColumnType("decimal(12,2)")
                .HasColumnName("total");

            builder.Property(e => e.CreatedAt)
                    .HasColumnName("createdAt")
                    .HasColumnType("date")
                    .HasDefaultValueSql("CURRENT_DATE")
                    .ValueGeneratedOnAdd();

            builder.Property(e => e.UpdatedAt)
                    .HasColumnName("updatedAt")
                    .HasColumnType("date")
                    .HasDefaultValueSql("CURRENT_DATE")
                    .ValueGeneratedOnAddOrUpdate();
        }
    }
}