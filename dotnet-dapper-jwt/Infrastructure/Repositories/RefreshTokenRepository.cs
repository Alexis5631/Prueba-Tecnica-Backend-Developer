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
    public class RefreshTokenRepository : GenericRepository<RefreshToken>, IRefreshTokenRepository
    {
        protected readonly DapperContext _context;

        public RefreshTokenRepository(DapperContext context) : base(context)
        {
            _context = context;
        }

        public override void Add(RefreshToken entity)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                INSERT INTO refresh_tokens (user_id, token, expires, created, revoked)
                VALUES (@UserId, @Token, @Expires, @Created, @Revoked)
                RETURNING id";
            
            var id = connection.QuerySingle<int>(sql, new
            {
                entity.UserId,
                entity.Token,
                entity.Expires,
                entity.Created,
                entity.Revoked
            });
            
            entity.Id = id;
        }

        public override void Update(RefreshToken entity)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                UPDATE refresh_tokens 
                SET user_id = @UserId, token = @Token, expires = @Expires, created = @Created, revoked = @Revoked
                WHERE id = @Id";
            
            connection.Execute(sql, new
            {
                entity.Id,
                entity.UserId,
                entity.Token,
                entity.Expires,
                entity.Created,
                entity.Revoked
            });
        }

        public override void Remove(RefreshToken entity)
        {
            using var connection = _context.CreateConnection();
            var sql = "DELETE FROM refresh_tokens WHERE id = @Id";
            connection.Execute(sql, new { entity.Id });
        }

        public override IEnumerable<RefreshToken> Find(System.Linq.Expressions.Expression<Func<RefreshToken, bool>> expression)
        {
            // For complex expressions, we'll need to implement a query builder
            // For now, return empty collection - specific methods should be used
            return new List<RefreshToken>();
        }

        public IEnumerable<RefreshToken> FindByUserId(int userId)
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT id, user_id, token, expires, created, revoked FROM refresh_tokens WHERE user_id = @UserId";
            return connection.Query<RefreshToken>(sql, new { UserId = userId });
        }

        public IEnumerable<RefreshToken> FindByToken(string token)
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT id, user_id, token, expires, created, revoked FROM refresh_tokens WHERE token = @Token";
            return connection.Query<RefreshToken>(sql, new { Token = token });
        }

        public IEnumerable<RefreshToken> FindActiveByUserId(int userId)
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT id, user_id, token, expires, created, revoked FROM refresh_tokens WHERE user_id = @UserId AND revoked IS NULL AND expires > NOW()";
            return connection.Query<RefreshToken>(sql, new { UserId = userId });
        }
    }
}