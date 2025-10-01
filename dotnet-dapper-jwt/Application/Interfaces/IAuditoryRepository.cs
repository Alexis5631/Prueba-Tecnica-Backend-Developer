using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Repositories
{
	public interface IAuditoryRepository : IRepository<Auditory>
	{
		Task<IReadOnlyList<Auditory>> ListByEntityAsync(string entityName, int recordId);
	}
}

