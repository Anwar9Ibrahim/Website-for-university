using System;
using Microsoft.EntityFrameworkCore;
using GroupC.Uni.Core.Entities;
using GroupC.Uni.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroupC.Uni.Infrastructure.Data
{
    /// <summary>
    /// "There's some repetition here - couldn't we have some the sync methods call the async?"
    /// https://blogs.msdn.microsoft.com/pfxteam/2012/04/13/should-i-expose-synchronous-wrappers-for-asynchronous-methods/
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EfRepository<T> : IAsyncRepository<T> where T : BaseEntity, IAggregateRoot
    {
        protected readonly AppDbContext _dbContext;

        public EfRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual async Task<T> GetByIdAsync(Guid id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }
        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }
        public async Task<IEnumerable<T>> ListAllAsyncNoReadOnly()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }
        public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        public async Task<int> CountAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            entity.CreationDate = System.DateTime.Now;
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            entity.LastUpdateDate = System.DateTime.Now;
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>().AsQueryable(), spec);
        }
        //Added By Us
        public List<T> ListAll()
        {
            return _dbContext.Set<T>().ToList();
        }
        public List<T> ListAllActivate()
        {
            return _dbContext.Set<T>().Where(s=> s.Status==MyEnums.status.Active).ToList();
        }
        public async Task Deactivate(T entity)
        {
            var _entity = await _dbContext.Set<T>().FindAsync(entity.Id);
            _entity.Status = MyEnums.status.NotActive;

            await _dbContext.SaveChangesAsync();
        }
        public int AllRecordsCount()
            {
            return _dbContext.Set<T>().ToList().Count();


        }
    }
}