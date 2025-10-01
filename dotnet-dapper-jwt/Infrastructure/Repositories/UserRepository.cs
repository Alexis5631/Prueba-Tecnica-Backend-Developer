using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
	public class UserRepository : GenericRepository<User>, IUserRepository
	{
		protected readonly PruebaDbContext _context;

		public UserRepository(PruebaDbContext context) : base(context)
		{
			_context = context;
		}

		public async Task<User?> GetByUsernameAsync(string username)
		{
			return await _context.Users
				.Include(u => u.Role)               // relación directa
				.Include(u => u.RefreshTokens)      // tokens
				.FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());
		}

		public async Task<User?> GetByRefreshTokenAsync(string refreshToken)
		{
			return await _context.Users
				.Include(u => u.Role)               // relación directa
				.Include(u => u.RefreshTokens)      // tokens
				.FirstOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == refreshToken));
		}
	}
}

