using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
	public class OrderRepository : Repository<Order>, IOrderRepository
	{
		public OrderRepository(AppDbContext dbContext) : base(dbContext)
		{
		}

		public async Task<IReadOnlyList<Order>> ListByUserAsync(int userId)
		{
			return await DbContext.Orders.AsNoTracking().Where(o => o.UserId == userId).ToListAsync();
		}
	}
}

