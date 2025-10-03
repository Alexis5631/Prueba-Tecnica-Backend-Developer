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
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        protected readonly DapperContext _context;

        public RoleRepository(DapperContext context) : base(context)
        {
            _context = context;
        }

        public override void Add(Role entity)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                INSERT INTO roles (name)
                VALUES (@Name)
                RETURNING id";
            
            var id = connection.QuerySingle<int>(sql, new
            {
                entity.Name
            });
            
            entity.Id = id;
        }

        public override void Update(Role entity)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                UPDATE roles 
                SET name = @Name
                WHERE id = @Id";
            
            connection.Execute(sql, new
            {
                entity.Id,
                entity.Name
            });
        }

        public override void Remove(Role entity)
        {
            using var connection = _context.CreateConnection();
            var sql = "DELETE FROM roles WHERE id = @Id";
            connection.Execute(sql, new { entity.Id });
        }

        public override IEnumerable<Role> Find(System.Linq.Expressions.Expression<Func<Role, bool>> expression)
        {
            // For complex expressions, we'll need to implement a query builder
            // For now, return empty collection - specific methods should be used
            return new List<Role>();
        }

        public IEnumerable<Role> FindByName(string name)
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT id, name FROM roles WHERE LOWER(name) = LOWER(@Name)";
            return connection.Query<Role>(sql, new { Name = name });
        }
    }
}