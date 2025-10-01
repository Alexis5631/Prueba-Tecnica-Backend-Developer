using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Repositories
{
	public interface IOrderItemRepository : IRepository<OrderItem>
	{
		Task<IReadOnlyList<OrderItem>> ListByOrderAsync(int orderId);
	}
}

