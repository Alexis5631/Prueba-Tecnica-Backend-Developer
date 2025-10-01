using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Configuration
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshTokenConfiguration>
    {
        public void Configure(EntityTypeBuilder<RefreshTokenConfiguration> builder)
        {
            builder.ToTable("refresh_tokens");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("id");

            builder.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .IsRequired();

            builder.Property(e => e.Token)
                    .HasColumnName("token")
                    .HasColumnType("text");

            builder.Property(e => e.Expires)
                    .HasColumnName("expires")
                    .HasColumnType("timestamp with time zone");

            builder.Property(e => e.Created)
                    .HasColumnName("created")
                    .HasColumnType("timestamp with time zone");

            builder.Property(e => e.Revoked)
                    .HasColumnName("revoked")
                    .HasColumnType("timestamp with time zone");

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

            builder.HasOne(e => e.User)
                    .WithMany(u => u.RefreshTokens)
                    .HasForeignKey(e => e.UserId);
        }
    }
}