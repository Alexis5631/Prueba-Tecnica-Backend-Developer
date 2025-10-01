using Domain.Entities;
using Application.Interfaces;

namespace Infrastructure.Repositories
{
	public class RoleRepository : IRoleRepository
	{
		protected readonly PruebaDbContext _context;

		public RoleRepository(PruebaDbContext context) : base(context)
		{
			_context = context;
		}
	}
}

