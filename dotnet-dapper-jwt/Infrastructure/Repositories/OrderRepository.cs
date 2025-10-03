using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Application.Interfaces;
using Infrastructure.Data;
using Dapper;

namespace Infrastructure.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        protected readonly DapperContext _context;

        public OrderRepository(DapperContext context) : base(context)
        {
            _context = context;
        }

        public override void Add(Order entity)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                INSERT INTO orders (user_id, total, created_at)
                VALUES (@UserId, @Total, @CreatedAt)
                RETURNING id";
            
            var id = connection.QuerySingle<int>(sql, new
            {
                entity.UserId,
                entity.Total,
                CreatedAt = DateTime.Now
            });
            
            entity.Id = id;
        }

        public override void Update(Order entity)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                UPDATE orders 
                SET user_id = @UserId, total = @Total
                WHERE id = @Id";
            
            connection.Execute(sql, new
            {
                entity.Id,
                entity.UserId,
                entity.Total
            });
        }

        public override void Remove(Order entity)
        {
            using var connection = _context.CreateConnection();
            var sql = "DELETE FROM orders WHERE id = @Id";
            connection.Execute(sql, new { entity.Id });
        }

        public override IEnumerable<Order> Find(System.Linq.Expressions.Expression<Func<Order, bool>> expression)
        {
            // For complex expressions, we'll need to implement a query builder
            // For now, return empty collection - specific methods should be used
            return new List<Order>();
        }

        public IEnumerable<Order> FindByUserId(int userId)
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT * FROM orders WHERE user_id = @UserId";
            return connection.Query<Order>(sql, new { UserId = userId });
        }

        public override async Task<Order?> GetByIdAsync(int id)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                SELECT o.*, u.username
                FROM orders o
                LEFT JOIN users u ON o.user_id = u.id
                WHERE o.id = @Id";
            
            var order = await connection.QueryFirstOrDefaultAsync<Order>(sql, new { Id = id });
            
            if (order != null)
            {
                // Load order items
                var orderItemsSql = @"
                    SELECT oi.*, p.name as product_name, p.sku as product_sku
                    FROM order_items oi
                    LEFT JOIN products p ON oi.product_id = p.id
                    WHERE oi.order_id = @OrderId";
                
                var orderItems = await connection.QueryAsync<OrderItem>(orderItemsSql, new { OrderId = id });
                order.OrderItems = orderItems.ToList();
            }
            
            return order;
        }
    }
}