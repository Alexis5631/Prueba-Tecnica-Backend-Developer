using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
	public class OrderItemRepository : GenericRepository<OrderItem>, IOrderItemRepository
	{
		protected readonly PruebaDbContext _context;

		public OrderItemRepository(PruebaDbContext context) : base(context)
		{
			_context = context;
		}
	}
}

