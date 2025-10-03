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
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        protected readonly DapperContext _context;

        public UserRepository(DapperContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                SELECT u.id, u.username, u.password_hash, u.role_id, u.created_at, u.updated_at, u.is_active,
                       r.id as role_id, r.name as role_name
                FROM users u
                LEFT JOIN roles r ON u.role_id = r.id
                WHERE LOWER(u.username) = LOWER(@Username)";
            
            var result = await connection.QueryFirstOrDefaultAsync(sql, new { Username = username });
            
            if (result == null) return null;
            
            var user = new User
            {
                Id = result.id,
                Username = result.username,
                PasswordHash = result.password_hash,
                RoleId = result.role_id,
                CreatedAt = DateOnly.FromDateTime(result.created_at),
                UpdatedAt = DateOnly.FromDateTime(result.updated_at),
                IsActive = result.is_active
            };
            
            if (result.role_id != null)
            {
                user.Role = new Role
                {
                    Id = result.role_id,
                    Name = result.role_name
                };
            }
            
            // Load refresh tokens
            var refreshTokenSql = @"
                SELECT id, user_id, token, expires, created, revoked
                FROM refresh_tokens 
                WHERE user_id = @UserId AND (revoked IS NULL AND expires > NOW())";
            var refreshTokens = await connection.QueryAsync<RefreshToken>(refreshTokenSql, new { UserId = user.Id });
            user.RefreshTokens = refreshTokens.ToList();
            
            return user;
        }

        public async Task<User?> GetByRefreshTokenAsync(string refreshToken)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                SELECT u.id, u.username, u.password_hash, u.role_id, u.created_at, u.updated_at, u.is_active,
                       r.id as role_id, r.name as role_name
                FROM users u
                LEFT JOIN roles r ON u.role_id = r.id
                WHERE u.id IN (SELECT user_id FROM refresh_tokens WHERE token = @Token AND revoked IS NULL AND expires > NOW())";
            
            var result = await connection.QueryFirstOrDefaultAsync(sql, new { Token = refreshToken });
            
            if (result == null) return null;
            
            var user = new User
            {
                Id = result.id,
                Username = result.username,
                PasswordHash = result.password_hash,
                RoleId = result.role_id,
                CreatedAt = DateOnly.FromDateTime(result.created_at),
                UpdatedAt = DateOnly.FromDateTime(result.updated_at),
                IsActive = result.is_active
            };
            
            if (result.role_id != null)
            {
                user.Role = new Role
                {
                    Id = result.role_id,
                    Name = result.role_name
                };
            }
            
            // Load refresh tokens
            var refreshTokenSql = @"
                SELECT id, user_id, token, expires, created, revoked
                FROM refresh_tokens 
                WHERE user_id = @UserId AND (revoked IS NULL AND expires > NOW())";
            var refreshTokens = await connection.QueryAsync<RefreshToken>(refreshTokenSql, new { UserId = user.Id });
            user.RefreshTokens = refreshTokens.ToList();
            
            return user;
        }

        public override void Add(User entity)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                INSERT INTO users (username, password_hash, role_id, created_at, updated_at, is_active)
                VALUES (@Username, @PasswordHash, @RoleId, @CreatedAt, @UpdatedAt, @IsActive)
                RETURNING id";
            
            var id = connection.QuerySingle<int>(sql, new
            {
                entity.Username,
                entity.PasswordHash,
                entity.RoleId,
                CreatedAt = entity.CreatedAt.ToDateTime(TimeOnly.MinValue),
                UpdatedAt = entity.UpdatedAt.ToDateTime(TimeOnly.MinValue),
                entity.IsActive
            });
            
            entity.Id = id;
        }

        public override void Update(User entity)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                UPDATE users 
                SET username = @Username, password_hash = @PasswordHash, role_id = @RoleId, 
                    updated_at = @UpdatedAt, is_active = @IsActive
                WHERE id = @Id";
            
            connection.Execute(sql, new
            {
                entity.Id,
                entity.Username,
                entity.PasswordHash,
                entity.RoleId,
                UpdatedAt = entity.UpdatedAt.ToDateTime(TimeOnly.MinValue),
                entity.IsActive
            });
        }

        public override void Remove(User entity)
        {
            using var connection = _context.CreateConnection();
            var sql = "DELETE FROM users WHERE id = @Id";
            connection.Execute(sql, new { entity.Id });
        }

        public override IEnumerable<User> Find(System.Linq.Expressions.Expression<Func<User, bool>> expression)
        {
            // For complex expressions, we'll need to implement a query builder
            // For now, return empty collection - specific methods should be used
            return new List<User>();
        }

        public override async Task<IEnumerable<User>> GetAllAsync()
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                SELECT u.id, u.username, u.password_hash, u.role_id, u.created_at, u.updated_at, u.is_active,
                       r.id as role_id, r.name as role_name
                FROM users u
                LEFT JOIN roles r ON u.role_id = r.id
                ORDER BY u.id";
            
            var results = await connection.QueryAsync(sql);
            
            return results.Select(result => new User
            {
                Id = result.id,
                Username = result.username,
                PasswordHash = result.password_hash,
                RoleId = result.role_id,
                CreatedAt = DateOnly.FromDateTime(result.created_at),
                UpdatedAt = DateOnly.FromDateTime(result.updated_at),
                IsActive = result.is_active,
                Role = result.role_id != null ? new Role
                {
                    Id = result.role_id,
                    Name = result.role_name
                } : null
            });
        }

        public override async Task<User?> GetByIdAsync(int id)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                SELECT u.id, u.username, u.password_hash, u.role_id, u.created_at, u.updated_at, u.is_active,
                       r.id as role_id, r.name as role_name
                FROM users u
                LEFT JOIN roles r ON u.role_id = r.id
                WHERE u.id = @Id";
            
            var result = await connection.QueryFirstOrDefaultAsync(sql, new { Id = id });
            
            if (result == null) return null;
            
            var user = new User
            {
                Id = result.id,
                Username = result.username,
                PasswordHash = result.password_hash,
                RoleId = result.role_id,
                CreatedAt = DateOnly.FromDateTime(result.created_at),
                UpdatedAt = DateOnly.FromDateTime(result.updated_at),
                IsActive = result.is_active
            };
            
            if (result.role_id != null)
            {
                user.Role = new Role
                {
                    Id = result.role_id,
                    Name = result.role_name
                };
            }
            
            return user;
        }

        public IEnumerable<User> FindByUsername(string username)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                SELECT u.id, u.username, u.password_hash, u.role_id, u.created_at, u.updated_at, u.is_active,
                       r.id as role_id, r.name as role_name
                FROM users u
                LEFT JOIN roles r ON u.role_id = r.id
                WHERE LOWER(u.username) = LOWER(@Username)";
            
            var results = connection.Query(sql, new { Username = username });
            
            return results.Select(result => new User
            {
                Id = result.id,
                Username = result.username,
                PasswordHash = result.password_hash,
                RoleId = result.role_id,
                CreatedAt = DateOnly.FromDateTime(result.created_at),
                UpdatedAt = DateOnly.FromDateTime(result.updated_at),
                IsActive = result.is_active,
                Role = result.role_id != null ? new Role
                {
                    Id = result.role_id,
                    Name = result.role_name
                } : null
            });
        }
    }
}