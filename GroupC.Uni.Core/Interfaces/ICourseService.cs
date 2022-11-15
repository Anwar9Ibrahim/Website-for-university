using GroupC.Uni.Core.Entities;
using GroupC.Uni.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GroupC.Uni.Core.Interfaces
{
    public interface ICourseService:IService<Course>
    {
        Task specialUpdateAsync(Course course);
        Task<IReadOnlyList<Course>> ListActiveSync();

        Task<IReadOnlyList<Course>> ListRecentCoursesSync();
    }
}
