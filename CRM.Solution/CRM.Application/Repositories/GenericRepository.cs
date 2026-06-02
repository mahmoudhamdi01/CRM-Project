using CRM.Infrastructure.Data;
using CRM.Infrastructure.Entities;
using CRM.Interface.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.Repositories
{
    public class GenericRepository<TEntity, TKey>(ApplicationDbContext _dbContext) : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        public IQueryable<TEntity> Query()
        => _dbContext.Set<TEntity>().AsQueryable();

        public async Task AddAsync(TEntity entity)
        => await _dbContext.AddAsync(entity);

        public async Task<TEntity?> GetByIdAsync(int id)
        => await _dbContext.Set<TEntity>().FindAsync(id);

        public void Remove(TEntity entity)
        => _dbContext.Remove(entity);

        public void Update(TEntity entity)
        => _dbContext.Update(entity);
    }
}
