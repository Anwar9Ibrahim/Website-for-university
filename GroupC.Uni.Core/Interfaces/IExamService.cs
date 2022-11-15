using GroupC.Uni.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
namespace GroupC.Uni.Core.Interfaces
{
    public interface IExamService : IService<Exam>
    {

        Task<IReadOnlyList<Exam>> ListAllAsyncWithExTT();
        Task specialUpdateAsync(Exam exam);
        Task<Exam> GetExamWithAttributes(Guid id);
        Task<IReadOnlyList<Exam>> GetLatestExamsAsync();

        Task<IReadOnlyList<Exam>> ListAllAsyncWithExams();
        Task<Exam> GetExamAsyncwithQuestions(Guid guid);
    }
}
