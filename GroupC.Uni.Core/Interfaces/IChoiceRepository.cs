using GroupC.Uni.Core.Entities;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
namespace GroupC.Uni.Core.Interfaces
{
    public interface IChoiceRepository :  IAsyncRepository<Choice>
    {
        Task<List<Choice>> listChoicesByQuestionId(Guid id);
        Task<Choice> getByIdWithQuestion(Guid id);
        // To be used in the Update view in Question controller
        Task specialUpdateAsync(Choice Choice);
        // To be used in the Index view in Question controller
        Task<IReadOnlyList<Choice>> ListActiveSyncWithQuestion();
    }
}
