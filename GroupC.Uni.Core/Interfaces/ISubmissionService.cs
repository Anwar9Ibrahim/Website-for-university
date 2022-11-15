using GroupC.Uni.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
namespace GroupC.Uni.Core.Interfaces
{
    public interface ISubmissionService : IService<Submission>
    {
        Task CreateSubmissionAsync(Submission Submission);
        Task<Submission> getByIdWithAll(Guid id);
        
        Task<IReadOnlyList<Submission>> ListActiveSyncWithAll();
        Task<List<Submission>> listSubmissionsByStudentId(Guid id);
        Task<List<Submission>> listSubmissionsByExamId(Guid id);
        new Task<IEnumerable<Submission>> ListAllAsyncNoReadOnly();
    }
}
