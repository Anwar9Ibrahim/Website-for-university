using GroupC.Uni.Core.Entities;
using GroupC.Uni.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Collections.Generic;
namespace GroupC.Uni.Infrastructure.Data
{
    public class QuestionRepository : EfRepository<Question>, IQuestionRepository
    {
        public QuestionRepository(AppDbContext dbContext) : base(dbContext)
        {
        }


        //Task<List<Question>> IQuestionRepository.ListQuestionByTopicId(Guid TopicId)
        //{
        //    return _dbContext.Questions.Where(s => s.TopicId == TopicId)
        //                   .Include(s => s.Topic)
        //                   .ToListAsync();
        //}
        public async Task<Question> GetByIdWithTopic(Guid id)
        {
            return await _dbContext.Questions
                .Include(q => q.Topic).Include(q=>q.Choices)
                .FirstOrDefaultAsync(m => m.Id == id);
        }
        public async Task specialUpdateAsync(Question question)
        {
            Question _question = await _dbContext.Set<Question>().FindAsync(question.Id);
            _question.IsHtml = question.IsHtml;
            _question.LastUpdateDate = DateTime.Now;
            _question.Mark = question.Mark;
            _question.Text = question.Text;
            _question.Choices = question.Choices;
            await _dbContext.SaveChangesAsync();
        }
        public async Task<IReadOnlyList<Question>> ListActiveSyncWithTopic()
            {
            return await _dbContext.Questions.Include(q => q.Topic).Include(q => q.Choices).Where(q => q.Status== MyEnums.status.Active).ToListAsync();
            }
        //For server side DataTable ,, count of filtered records
        public int RecordsFilteredCount(string valueToSearch)
        {
            return _dbContext.Questions
                    .Where(a => a.Status == MyEnums.status.Active && ( a.Text.IndexOf(valueToSearch, StringComparison.OrdinalIgnoreCase) >= 0 || a.Topic.Name.IndexOf(valueToSearch, StringComparison.OrdinalIgnoreCase) >= 0))
                    .Count();

        }
        public List<Question> FilteredDataAsc(string valueToSearch, string sortColumnName,int start,int length)
        {
            return _dbContext.Questions
                    .Where(a => a.Status == MyEnums.status.Active && ( a.Text.IndexOf(valueToSearch, StringComparison.OrdinalIgnoreCase) >= 0 || a.Topic.Name.IndexOf(valueToSearch, StringComparison.OrdinalIgnoreCase) >= 0))
                    .Include(q => q.Topic).Include(q => q.Choices)
                    .OrderBy(x => x.GetType().GetProperty(sortColumnName).GetValue(x))//Sort by sortColumn
                    .Skip(start)
                    .Take(length)
                    .ToList<Question>();
        }
        public List<Question> FilteredDataDesc(string valueToSearch, string sortColumnName, int start, int length)
        {
            return
                   _dbContext.Questions
                    .Where(a => a.Status==  MyEnums.status.Active && (a.Text.IndexOf(valueToSearch, StringComparison.OrdinalIgnoreCase) >= 0 || a.Topic.Name.IndexOf(valueToSearch, StringComparison.OrdinalIgnoreCase) >= 0))
                    .Include(q => q.Topic).Include(q => q.Choices)
                    .OrderByDescending(x => x.GetType().GetProperty(sortColumnName).GetValue(x))
                   .Skip(start)
                   .Take(length)
                   .ToList<Question>();
        }
        public List<Question> GetByTopicIdWithTopic(Guid topicId)
        {
            return  _dbContext.Questions.Where(a => a.Status == MyEnums.status.Active && a.TopicId== topicId)
               .Include(q => q.Topic).ToList<Question>();
        }
    }
}
