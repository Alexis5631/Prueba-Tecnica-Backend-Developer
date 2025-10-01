using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Repositories
{
	public interface IProductRepository : IRepository<Product>
	{
		Task<Product?> GetBySkuAsync(string sku);
	}
}

