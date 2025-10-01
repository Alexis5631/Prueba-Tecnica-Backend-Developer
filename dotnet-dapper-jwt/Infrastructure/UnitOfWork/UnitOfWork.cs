using System.Threading;
using System.Threading.Tasks;
using Domain.Repositories;
using Infrastructure.Repositories;

namespace Infrastructure.UnitOfWork
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly AppDbContext _dbContext;

		private IRoleRepository _roles;
		private IUserRepository _users;
		private IProductRepository _products;
		private IOrderRepository _orders;
		private IOrderItemRepository _orderItems;
		private IAuditoryRepository _audits;

		public UnitOfWork(AppDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public IRoleRepository Roles => _roles ??= new RoleRepository(_dbContext);
		public IUserRepository Users => _users ??= new UserRepository(_dbContext);
		public IProductRepository Products => _products ??= new ProductRepository(_dbContext);
		public IOrderRepository Orders => _orders ??= new OrderRepository(_dbContext);
		public IOrderItemRepository OrderItems => _orderItems ??= new OrderItemRepository(_dbContext);
		public IAuditoryRepository Audits => _audits ??= new AuditoryRepository(_dbContext);

		public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			return _dbContext.SaveChangesAsync(cancellationToken);
		}

		public void Dispose()
		{
			_dbContext.Dispose();
		}
	}
}

