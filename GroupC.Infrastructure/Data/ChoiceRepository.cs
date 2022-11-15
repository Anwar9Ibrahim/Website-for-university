using GroupC.Uni.Core.Entities;
using GroupC.Uni.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Collections.Generic;
namespace GroupC.Uni.Infrastructure.Data
{
    public class ChoiceRepository : EfRepository<Choice>, IChoiceRepository
    {
        public ChoiceRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Choice> getByIdWithQuestion(Guid id)
        {
            return await _dbContext.Choices
                .Include(q => q.Question)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IReadOnlyList<Choice>> ListActiveSyncWithQuestion()
        {
            return await _dbContext.Choices.Include(q => q.Question).Where(q => q.Status == MyEnums.status.Active).ToListAsync();

        }

        public Task<List<Choice>> listChoicesByQuestionId(Guid id)
        {
            return _dbContext.Choices.Where(s => s.QuestionId == id)
                           .Include(s => s.Question)
                           .ToListAsync();
        }

        public async Task specialUpdateAsync(Choice Choice)
        {
            Choice _choice = await _dbContext.Set<Choice>().FindAsync(Choice.Id);
           
            _choice.LastUpdateDate = DateTime.Now;
            _choice.Type = Choice.Type;
            _choice.Text = Choice.Text;
            await _dbContext.SaveChangesAsync();
        }


    }
}