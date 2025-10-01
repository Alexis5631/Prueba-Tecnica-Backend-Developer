using System.Threading.Tasks;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
	public class UserRepository : Repository<User>, IUserRepository
	{
		public UserRepository(AppDbContext dbContext) : base(dbContext)
		{
		}

		public async Task<User?> GetByUsernameAsync(string username)
		{
			return await DbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Username == username);
		}
	}
}

