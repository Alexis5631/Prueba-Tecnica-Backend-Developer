using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Domain.Entities;

namespace Infrastructure.Data
{
	public class PruebaDbContext : DbContext
	{
		public PruebaDbContext(DbContextOptions<PruebaDbContext> options) : base(options)
		{
		}

		public DbSet<Role> Roles { get; set; } = null!;
		public DbSet<User> Users { get; set; } = null!;
		public DbSet<Product> Products { get; set; } = null!;
		public DbSet<Order> Orders { get; set; } = null!;
		public DbSet<OrderItem> OrderItems { get; set; } = null!;

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
		}
    }
}