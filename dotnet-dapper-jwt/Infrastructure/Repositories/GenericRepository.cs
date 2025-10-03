using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Dapper;

namespace Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly DapperContext _context;

        public GenericRepository(DapperContext context)
        {
            _context = context;
        }

        protected virtual string TableName => typeof(T).Name.ToLower() + "s";

        public virtual void Add(T entity)
        {
            // This will be implemented in specific repositories
            throw new NotImplementedException("Use specific repository Add methods");
        }

        public virtual void AddRange(IEnumerable<T> entities)
        {
            // This will be implemented in specific repositories
            throw new NotImplementedException("Use specific repository AddRange methods");
        }

        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> expression)
        {
            // This will be implemented in specific repositories
            throw new NotImplementedException("Use specific repository Find methods");
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            using var connection = _context.CreateConnection();
            var sql = $"SELECT * FROM {TableName}";
            return await connection.QueryAsync<T>(sql);
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            using var connection = _context.CreateConnection();
            var sql = $"SELECT * FROM {TableName} WHERE id = @Id";
            return await connection.QueryFirstOrDefaultAsync<T>(sql, new { Id = id });
        }

        public virtual Task<T> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public virtual void Remove(T entity)
        {
            // This will be implemented in specific repositories
            throw new NotImplementedException("Use specific repository Remove methods");
        }

        public virtual void RemoveRange(IEnumerable<T> entities)
        {
            // This will be implemented in specific repositories
            throw new NotImplementedException("Use specific repository RemoveRange methods");
        }

        public virtual void Update(T entity)
        {
            // This will be implemented in specific repositories
            throw new NotImplementedException("Use specific repository Update methods");
        }
    }
}