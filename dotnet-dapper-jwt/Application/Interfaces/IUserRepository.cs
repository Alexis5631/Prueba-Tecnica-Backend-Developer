using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Repositories
{
	public interface IUserRepository : IRepository<User>
	{
		Task<User?> GetByUsernameAsync(string username);
	}
}

