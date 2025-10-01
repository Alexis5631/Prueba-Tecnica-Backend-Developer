using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
	public class OrderItemRepository : Repository<OrderItem>, IOrderItemRepository
	{
		public OrderItemRepository(AppDbContext dbContext) : base(dbContext)
		{
		}

		public async Task<IReadOnlyList<OrderItem>> ListByOrderAsync(int orderId)
		{
			return await DbContext.OrderItems.AsNoTracking().Where(oi => oi.OrderId == orderId).ToListAsync();
		}
	}
}

