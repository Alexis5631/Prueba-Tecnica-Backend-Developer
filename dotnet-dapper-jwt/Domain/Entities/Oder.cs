using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
	public class Order : BaseEntity
	{
		public int Id { get; set; }
		public int? UserId { get; set; }
		public decimal Total { get; set; }
		public DateTime CreatedAt { get; set; }

		public User? User { get; set; }
		public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
	}
}