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
    public class OrderItemRepository : GenericRepository<OrderItem>, IOrderItemRepository
    {
        protected readonly DapperContext _context;

        public OrderItemRepository(DapperContext context) : base(context)
        {
            _context = context;
        }

        public override void Add(OrderItem entity)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                INSERT INTO order_items (order_id, product_id, quantity, unit_price)
                VALUES (@OrderId, @ProductId, @Quantity, @UnitPrice)
                RETURNING id";
            
            var id = connection.QuerySingle<int>(sql, new
            {
                entity.OrderId,
                entity.ProductId,
                entity.Quantity,
                entity.UnitPrice
            });
            
            entity.Id = id;
        }

        public override void Update(OrderItem entity)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                UPDATE order_items 
                SET order_id = @OrderId, product_id = @ProductId, quantity = @Quantity, 
                    unit_price = @UnitPrice
                WHERE id = @Id";
            
            connection.Execute(sql, new
            {
                entity.Id,
                entity.OrderId,
                entity.ProductId,
                entity.Quantity,
                entity.UnitPrice
            });
        }

        public override void Remove(OrderItem entity)
        {
            using var connection = _context.CreateConnection();
            var sql = "DELETE FROM order_items WHERE id = @Id";
            connection.Execute(sql, new { entity.Id });
        }

        public override IEnumerable<OrderItem> Find(System.Linq.Expressions.Expression<Func<OrderItem, bool>> expression)
        {
            // For complex expressions, we'll need to implement a query builder
            // For now, return empty collection - specific methods should be used
            return new List<OrderItem>();
        }

        public IEnumerable<OrderItem> FindByOrderId(int orderId)
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT * FROM order_items WHERE order_id = @OrderId";
            return connection.Query<OrderItem>(sql, new { OrderId = orderId });
        }

        public IEnumerable<OrderItem> FindByProductId(int productId)
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT * FROM order_items WHERE product_id = @ProductId";
            return connection.Query<OrderItem>(sql, new { ProductId = productId });
        }
    }
}