using System.Linq;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces;

namespace Infrastructure.Repositories
{
    public class RefreshTokenRepository : GenericRepository<RefreshToken>, IRefreshTokenRepository
    {
        protected readonly PruebaDbContext _context;

        public RefreshTokenRepository(PruebaDbContext context) : base(context)
        {
            _context = context;
        }
    }
}