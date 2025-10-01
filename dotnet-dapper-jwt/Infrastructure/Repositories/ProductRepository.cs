using System.Threading.Tasks;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
	public class ProductRepository : Repository<Product>, IProductRepository
	{
		public ProductRepository(AppDbContext dbContext) : base(dbContext)
		{
		}

		public async Task<Product?> GetBySkuAsync(string sku)
		{
			return await DbContext.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Sku == sku);
		}
	}
}

