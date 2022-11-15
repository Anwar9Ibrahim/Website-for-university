using GroupC.Uni.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GroupC.Uni.Core.Interfaces
{
    public interface IQuestionService: IService<Question>
    {
        Task CreateQuestionAsync(Question question);
        //Task<Question> GetByIdAsync(Guid id);
        Task<Question> GetByIdWithTopic(Guid id);
        Task specialUpdateAsync(Question question);

        // To be used in the Index view in Question controller
        Task<IReadOnlyList<Question>> ListActiveSyncWithTopic();
        //to be used in create in choice controller
        //List<Question> getAllAsList();
        //to be called in dataTable server-side
        int recordsFilteredCount(string valueToSearch);
        List<Question> FilteredDataAsc(string valueToSearch, string sortColumnName, int start, int length);
        List<Question> FilteredDataDesc(string valueToSearch, string sortColumnName, int start, int length);
        List<Question> GetByTopicIdWithTopic(Guid topicId);
    }
}
