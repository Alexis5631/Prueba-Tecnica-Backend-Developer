using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
	public class Auditory : BaseEntity
	{
		public int Id { get; set; }
		public string EntityName { get; set; } = default!;
		public int? RecordId { get; set; }
		public string ChangeType { get; set; } = default!;
		public int? ChangedBy { get; set; }
		public string? ChangedByUsername { get; set; }
		public DateTime OccurredAt { get; set; }
		public string? OldValues { get; set; }
		public string? NewValues { get; set; }
	}
}