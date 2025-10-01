using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
	public class OrderRepository : IOrderRepository
	{
		protected readonly PruebaDbContext _context;

		public OrderRepository(PruebaDbContext context) : base(context)
		{
			_context = context;
		}
	}
}

