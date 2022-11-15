using GroupC.Uni.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GroupC.Uni.Core.Interfaces
{
    public interface ITopicService : IService<Topic>
    {
        //Task<List<string>> ListTopicNamesAsync();
        Task CreateTopicAsync(Topic Topic);
        Task<Topic> GetByIdWithCourse(Guid id);
        Task SpecialUpdateAsync(Topic Topic);

        Task<IReadOnlyList<Topic>> ListActiveSyncWithCourse();
        List<Topic> ListActiveTopicByCourseId(Guid courseId);
    }
}
