using GroupC.Uni.Core.Entities;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
namespace GroupC.Uni.Core.Interfaces
{
    public interface ISubmissionRepository : IAsyncRepository<Submission>
    {
        Task<List<Submission>> listSubmissionsByStudentId(Guid id);
        Task<List<Submission>> listSubmissionsByExamId(Guid id);
        Task<Submission> getByIdWithAll(Guid id);
        Task<IReadOnlyList<Submission>> ListActiveSyncWithAll();
        new Task<IEnumerable<Submission>> ListAllAsyncNoReadOnly();
    }
}
