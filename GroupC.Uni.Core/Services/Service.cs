using System;
using Microsoft.EntityFrameworkCore;
using GroupC.Uni.Core.Entities;
using GroupC.Uni.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroupC.Uni.Core.Services
{
    public class Service<T> : IService<T> where T : BaseEntity, IAggregateRoot
    {
        protected readonly IAsyncRepository<T> _repositry;

        public Service(IAsyncRepository<T> repositry)
        {
            _repositry = repositry;
        }
        public async Task<T> AddAsync(T entity)
        {
            return await _repositry.AddAsync(entity);
        }

        public async Task<int> CountAsync(ISpecification<T> spec)
        {
            return await _repositry.CountAsync(spec);
        }

        public async Task DeleteAsync(T entity)
        {
            await _repositry.DeleteAsync(entity);
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _repositry.GetByIdAsync(id);
        }

        public List<T> GetAllAsList()
        {
            return _repositry.ListAllActivate();
        }

        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await _repositry.ListAllAsync();
        }

        public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
        {
            return await _repositry.ListAsync(spec);
        }

        public async Task UpdateAsync(T entity)
        {
            await _repositry.UpdateAsync(entity);
        }
        public async Task Deactivate(T entity)
        {
            await _repositry.Deactivate(entity);
        }
        public int AllRecordsCount()
        {
            return _repositry.AllRecordsCount();
        }
        public async Task<IEnumerable<T>> ListAllAsyncNoReadOnly()
        {
            return await _repositry.ListAllAsyncNoReadOnly();
        }

    }
}
