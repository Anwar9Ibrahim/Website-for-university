using GroupC.Uni.Core.Entities;
using GroupC.Uni.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GroupC.Uni.Core.Services
{
    public class CourseService : Service<Course>, ICourseService
    {
        private readonly ICourseRepository _courseRepositry;
        private readonly IAppLogger<ICourseService> _logger;

        public CourseService(ICourseRepository courseRepositry, IAppLogger<ICourseService> logger) : base(courseRepositry)
        {
            _courseRepositry = courseRepositry;
            _logger = logger;
        }
        public async Task specialUpdateAsync(Course course)
        {
            await _courseRepositry.specialUpdateAsync(course);
        }
        public async Task<IReadOnlyList<Course>> ListActiveSync()
        {
            return await _courseRepositry.ListActiveSync();
        }
        public async Task<IReadOnlyList<Course>> ListRecentCoursesSync()
        {
            return await _courseRepositry.ListRecentCoursesSync();
        }
     

    }
}
