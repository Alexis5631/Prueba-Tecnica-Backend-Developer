using System.Threading.Tasks;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
	public class ProductRepository : IProductRepository
	{
		protected readonly PruebaDbContext _context;

		public ProductRepository(PruebaDbContext context) : base(context)
		{
			_context = context;
		}
	}
}

