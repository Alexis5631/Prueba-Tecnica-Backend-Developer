using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
	public class OrderItemRepository : IOrderItemRepository
	{
		protected readonly PruebaDbContext _context;

		public OrderItemRepository(PruebaDbContext context) : base(context)
		{
			_context = context;
		}
	}
}

