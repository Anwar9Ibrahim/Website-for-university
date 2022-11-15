using GroupC.Uni.Core.Entities;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace GroupC.Uni.Core.Interfaces
{
    public interface IExamRepository : IAsyncRepository<Exam>
    {
        Task<IReadOnlyList<Exam>> ListAllAsyncWithExTT();
        Task specialUpdateAsync(Exam exam);
        Task<Exam> GetExamWithAttributes(Guid id);
        Task<IReadOnlyList<Exam>> GetLatestExamsAsync();
        Task<IReadOnlyList<Exam>> ListAllAsyncWithExams();
        Task<Exam> GetExamAsyncwithQuestions(Guid guid);

    }
}
