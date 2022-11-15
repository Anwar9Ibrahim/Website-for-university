using GroupC.Uni.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GroupC.Uni.Core.Interfaces
{
    public interface IChoiceService: IService<Choice>
    {
        Task CreateChoiceAsync(Choice choice);
        Task<Choice> getByIdWithQuestion(Guid id);
        Task specialUpdateAsync(Choice Choice);
        
        Task<IReadOnlyList<Choice>> ListActiveSyncWithQuestion();

        Task<List<Choice>> listChoicesByQuestionId(Guid questionId);

    }
}
