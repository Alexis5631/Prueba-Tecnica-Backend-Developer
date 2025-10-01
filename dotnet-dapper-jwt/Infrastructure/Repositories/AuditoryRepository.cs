using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
	public class AuditoryRepository : Repository<Auditory>, IAuditoryRepository
	{
		public AuditoryRepository(AppDbContext dbContext) : base(dbContext)
		{
		}

		public async Task<IReadOnlyList<Auditory>> ListByEntityAsync(string entityName, int recordId)
		{
			return await DbContext.Audits.AsNoTracking()
				.Where(a => a.EntityName == entityName && a.RecordId == recordId)
				.OrderByDescending(a => a.OccurredAt)
				.ToListAsync();
		}
	}
}

