using Application.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;

namespace Infrastructure.UnitOfWork
{
	public class UnitOfWork : IUnitOfWork, IDisposable
	{
		private IRoleRepository? _roleRepository;
		private IUserRepository? _userRepository;
		private IProductRepository? _productRepository;
		private IOrderRepository? _orderRepository;
		private IOrderItemRepository? _orderItemRepository;
		private IRefreshTokenRepository? _refreshTokenRepository;

		private readonly PruebaDbContext _context;

		public UnitOfWork(PruebaDbContext context)
		{
			_context = context;
		}

		public IRoleRepository RoleRepository
		{
			get
			{
				if (_roleRepository == null)
				{
					_roleRepository = new RoleRepository(_context);
				}
				return _roleRepository;
			}
		}

		public IProductRepository ProductRepository
		{
			get
			{
				if (_productRepository == null)
				{
					_productRepository = new ProductRepository(_context);
				}
				return _productRepository;
			}
		}

		public IOrderRepository OrderRepository
		{
			get
			{
				if (_orderRepository == null)
				{
					_orderRepository = new OrderRepository(_context);
				}
				return _orderRepository;
			}
		}

		public IOrderItemRepository OrderItemRepository
		{
			get
			{
				if (_orderItemRepository == null)
				{
					_orderItemRepository = new OrderItemRepository(_context);
				}
				return _orderItemRepository;
			}
		}

		public IRefreshTokenRepository RefreshTokenRepository
		{
			get
			{
				if (_refreshTokenRepository == null)
				{
					_refreshTokenRepository = new RefreshTokenRepository(_context);
				}
				return _refreshTokenRepository;
			}
		}	
		public IUserRepository UserRepository
		{
			get
			{
				if (_userRepository == null)
				{
					_userRepository = new UserRepository(_context);
				}
				return _userRepository;
			}
		}	
		
		public async Task<int> SaveAsync()
		{
			return await _context.SaveChangesAsync();
		}

        public void Dispose()
        {
            _context.Dispose();
        }
	}
}

