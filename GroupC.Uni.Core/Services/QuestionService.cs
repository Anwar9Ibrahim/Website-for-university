using GroupC.Uni.Core.Entities;
using GroupC.Uni.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GroupC.Uni.Core.Services
{
    public class QuestionService : Service<Question>, IQuestionService
    {
        private readonly IQuestionRepository _questionRepositry;
        private readonly IAppLogger<IQuestionService> _logger;

        public QuestionService(IQuestionRepository questionRepositry, IAppLogger<IQuestionService> logger) : base(questionRepositry)
        {
            _questionRepositry = questionRepositry;
            _logger = logger;
        }
        public async Task CreateQuestionAsync(Question question)
        {
            await _questionRepositry.AddAsync(question);
        }
        //public async Task<Question> GetByIdAsync(Guid id)
        //{
        //    return await _questionRepositry.GetByIdAsync(id);
        //}
        public async Task<Question> GetByIdWithTopic(Guid id)
        {
            return await _questionRepositry.GetByIdWithTopic(id);
        }
        public async Task specialUpdateAsync(Question question)
        {
            await _questionRepositry.specialUpdateAsync(question);
        }
        public async Task<IReadOnlyList<Question>> ListActiveSyncWithTopic()
        {
            return await _questionRepositry.ListActiveSyncWithTopic();
        }
        //public List<Question> getAllAsList()
        //{
        //    return _questionRepositry.ListAll();
        //}
        //to be called in dataTable server-side
        public int recordsFilteredCount(string valueToSearch)
        {
            return _questionRepositry.RecordsFilteredCount(valueToSearch);
        }
      

        List<Question> IQuestionService.FilteredDataAsc(string valueToSearch, string sortColumnName, int start, int length)
        {
            return _questionRepositry.FilteredDataAsc(valueToSearch, sortColumnName, start, length);

        }

        List<Question> IQuestionService.FilteredDataDesc(string valueToSearch, string sortColumnName, int start, int length)
        {
            return _questionRepositry.FilteredDataDesc(valueToSearch, sortColumnName, start, length);

        }
        List<Question> GetByTopicIdWithTopic(Guid topicId)
        {
            return GetByTopicIdWithTopic( topicId);
        }

        List<Question> IQuestionService.GetByTopicIdWithTopic(Guid topicId)
        {
            throw new NotImplementedException();
        }
    }
}
