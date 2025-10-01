using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Repositories
{
	public interface IOrderRepository : IRepository<Order>
	{
		Task<IReadOnlyList<Order>> ListByUserAsync(int userId);
	}
}

