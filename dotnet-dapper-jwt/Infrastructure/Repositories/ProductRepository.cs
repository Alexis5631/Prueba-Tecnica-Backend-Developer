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
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        protected readonly DapperContext _context;

        public ProductRepository(DapperContext context) : base(context)
        {
            _context = context;
        }

        public override void Add(Product entity)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                INSERT INTO products (name, sku, price, stock, category)
                VALUES (@Name, @Sku, @Price, @Stock, @Category)
                RETURNING id";
            
            var id = connection.QuerySingle<int>(sql, new
            {
                entity.Name,
                entity.Sku,
                entity.Price,
                entity.Stock,
                entity.Category
            });
            
            entity.Id = id;
        }

        public override void Update(Product entity)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                UPDATE products 
                SET name = @Name, sku = @Sku, price = @Price, stock = @Stock, 
                    category = @Category
                WHERE id = @Id";
            
            connection.Execute(sql, new
            {
                entity.Id,
                entity.Name,
                entity.Sku,
                entity.Price,
                entity.Stock,
                entity.Category
            });
        }

        public override void Remove(Product entity)
        {
            using var connection = _context.CreateConnection();
            var sql = "DELETE FROM products WHERE id = @Id";
            connection.Execute(sql, new { entity.Id });
        }

        public override IEnumerable<Product> Find(System.Linq.Expressions.Expression<Func<Product, bool>> expression)
        {
            // For complex expressions, we'll need to implement a query builder
            // For now, return empty collection - specific methods should be used
            return new List<Product>();
        }

        public IEnumerable<Product> FindBySku(string sku)
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT * FROM products WHERE sku = @Sku";
            return connection.Query<Product>(sql, new { Sku = sku });
        }

        public IEnumerable<Product> FindByCategory(string category)
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT * FROM products WHERE category = @Category";
            return connection.Query<Product>(sql, new { Category = category });
        }

        public IEnumerable<Product> FindByName(string name)
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT * FROM products WHERE LOWER(name) LIKE LOWER(@Name)";
            return connection.Query<Product>(sql, new { Name = $"%{name}%" });
        }
    }
}