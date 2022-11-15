using GroupC.Uni.Core.Entities;
using GroupC.Uni.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
namespace GroupC.Uni.Infrastructure.Data
{
    public class TopicRepository: EfRepository<Topic>, ITopicRepository
    {
        public TopicRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Topic> GetByIdWithCourse(Guid id)
        {
            return await _dbContext.Topics
                .Include(q => q.Course)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        //public Task<List<Topic> > ListTopicByCourseId(Guid id)
        //{
        //    //implement the function to get all topics for a Course
        //    /////////////
        //    return _dbContext.Topics.Where(s => s.CourseId == id)
        //                   .Include(s => s.Course)
        //                   .ToListAsync();
        //}
        public Task<Guid> GetTopicIdByName(string topicName)
        {
            return _dbContext.Topics
                .Where(t => t.Name == topicName)
                .Select(t => t.Id).SingleAsync();
        }

        public async Task<IReadOnlyList<Topic>> ListActiveSyncWithCourse()
        {
            return await _dbContext.Topics.Include(q => q.Course).Where(q => q.Status == MyEnums.status.Active).ToListAsync();
        }

        public Task<List<Topic>> ListTopicsByCourseId(Guid id)
        {
            return _dbContext.Topics.Where(s => s.CourseId == id)
                           .Include(s => s.Course)
                           .ToListAsync();
        }

        public async Task SpecialUpdateAsync(Topic Topic)
        {
            Topic _Topic = await _dbContext.Set<Topic>().FindAsync(Topic.Id);

            _Topic.LastUpdateDate = DateTime.Now;
            _Topic.Name = Topic.Name;
           
            await _dbContext.SaveChangesAsync();
        }
        public List<Topic> ListActiveTopicByCourseId(Guid courseId)
        {
            return  _dbContext.Topics.Where(v => v.CourseId == courseId && v.Status == MyEnums.status.Active).ToList();
        }
    }
}
