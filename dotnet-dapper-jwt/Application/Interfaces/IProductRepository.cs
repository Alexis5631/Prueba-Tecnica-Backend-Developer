using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Interfaces
{
	public interface IProductRepository : IGenericRepository<Product>
	{
        IEnumerable<Product> FindBySku(string sku);
        IEnumerable<Product> FindByCategory(string category);
        IEnumerable<Product> FindByName(string name);
	}
}

