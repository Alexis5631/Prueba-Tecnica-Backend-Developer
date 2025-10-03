using System.Collections.Generic;
using Domain.Entities;

namespace Application.Interfaces
{
	public interface IRoleRepository : IGenericRepository<Role>
	{
        IEnumerable<Role> FindByName(string name);
	}
}

