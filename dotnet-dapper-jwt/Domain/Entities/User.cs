using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
	public class User : BaseEntity
	{
		public int Id { get; set; }
		public string? Username { get; set; }
		public string? PasswordHash { get; set; }
		public int? RoleId { get; set; }
		public bool IsActive { get; set; } = true;

		public Role? Role { get; set; }

		public ICollection<RefreshToken> RefreshTokens { get; set; } = new HashSet<RefreshToken>();
		public ICollection<Order> Orders { get; set; } = new HashSet<Order>();
	}
}

