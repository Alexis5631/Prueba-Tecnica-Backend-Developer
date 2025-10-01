using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories
{
	public class RoleRepository : Repository<Role>, IRoleRepository
	{
		public RoleRepository(AppDbContext dbContext) : base(dbContext)
		{
		}
	}
}

