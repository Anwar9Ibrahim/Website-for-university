using System;
using System.Collections.Generic;
using GroupC.Uni.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace GroupC.Uni.Core.Interfaces
{
    public interface IService<T> where T : BaseEntity, IAggregateRoot
    {
        Task<T> GetByIdAsync(Guid id);
        Task<IReadOnlyList<T>> ListAllAsync();
        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<int> CountAsync(ISpecification<T> spec);
        List<T> GetAllAsList();
        Task Deactivate(T entity);
        int AllRecordsCount();
        Task<IEnumerable<T>> ListAllAsyncNoReadOnly();
    }
}
