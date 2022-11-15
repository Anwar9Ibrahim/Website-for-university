using GroupC.Uni.Core.Entities;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace GroupC.Uni.Core.Interfaces
{
    public interface IQuestionRepository : IAsyncRepository<Question>
    {

        //Task<List<Question>> ListQuestionByTopicId(Guid id);

        // To be used in the Edit,Details,Delet,DeletConfirmed views in Question controller
        Task<Question> GetByIdWithTopic(Guid id);
        // To be used in the Update view in Question controller
        Task specialUpdateAsync(Question question);
        // To be used in the Index view in Question controller
        Task<IReadOnlyList<Question>> ListActiveSyncWithTopic();
        //for datatabel server-side 
         int RecordsFilteredCount(string valueToSearch);
         List<Question> FilteredDataAsc(string valueToSearch, string sortColumnName, int start, int length);
         List<Question> FilteredDataDesc(string valueToSearch, string sortColumnName, int start, int length);
        List<Question> GetByTopicIdWithTopic(Guid topicId);
        }
}

