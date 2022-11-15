using GroupC.Uni.Core.Entities;
using GroupC.Uni.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Collections.Generic;

namespace GroupC.Uni.Infrastructure.Data
{
    public class CourseRepository : EfRepository<Course>, ICourseRepository
    {
        public CourseRepository(AppDbContext dbContext) : base(dbContext)
        {

        }
        public async Task specialUpdateAsync(Course course)
        {
            Course _course = await _dbContext.Set<Course>().FindAsync(course.Id);
            _course.Name = course.Name;
            _course.Code = course.Code;
            _course.ImageURL = course.ImageURL;
            _course.Topics = new List<Topic>();
            _course.Topics = course.Topics;
            _course.CreationDate = course.CreationDate;
            _course.Exams = course.Exams;
            _course.Status = course.Status;
            _course.LastUpdateDate = DateTime.Now;
            await _dbContext.SaveChangesAsync();
        }
        public async Task<IReadOnlyList<Course>> ListActiveSync()
        {
            return await _dbContext.Courses.Where(q => q.Status == MyEnums.status.Active).ToListAsync();
        }

        public async Task<IReadOnlyList<Course>> ListRecentCoursesSync()
        {
            return await _dbContext.Courses.Where(q => q.Status == MyEnums.status.Active).OrderByDescending(d => d.CreationDate).Take(5).ToListAsync();

        }

    }
}
