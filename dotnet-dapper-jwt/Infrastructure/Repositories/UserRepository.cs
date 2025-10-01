using System.Threading.Tasks;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
	public class UserRepository : IUserRepository
	{
		protected readonly PruebaDbContext _context;

		public UserRepository(PruebaDbContext context) : base(context)
		{
			_context = context;
		}

		public async Task<User?> GetByUsernameAsync(string username)
		{
			return await DbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Username == username);
		}
	}
}

