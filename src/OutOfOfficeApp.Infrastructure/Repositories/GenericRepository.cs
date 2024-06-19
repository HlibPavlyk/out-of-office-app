using Microsoft.EntityFrameworkCore;
using OutOfOfficeApp.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OutOfOfficeApp.Infrastructure.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected readonly ApplicationDbContext _context;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<TEntity?> GetByIdAsync(int id)
        {
            return await _context.Set<TEntity>()
                .FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>?> GetAllAsync()
        {
            return await _context.Set<TEntity>()
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>()
                .AddAsync(entity);
        }

        public async Task Remove(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null)
            {
                throw new ArgumentNullException("Entity not found");
            }
            _context.Set<TEntity>().Remove(entity);
        }

        public void Update(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
        }
    }
}
