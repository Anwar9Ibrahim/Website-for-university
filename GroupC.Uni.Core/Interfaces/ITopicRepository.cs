using GroupC.Uni.Core.Entities;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
namespace GroupC.Uni.Core.Interfaces
{
    public interface ITopicRepository : IAsyncRepository<Topic>
    {

        //Task<List<Topic>> ListTopicByCourseId(Guid id);
        Task<Guid> GetTopicIdByName(string topicName);
        Task<List<Topic>> ListTopicsByCourseId(Guid id);
        Task<Topic> GetByIdWithCourse(Guid id);
        // To be used in the Update view in Course controller
        Task SpecialUpdateAsync(Topic Topic);
        // To be used in the Index view in Course controller
        Task<IReadOnlyList<Topic>> ListActiveSyncWithCourse();

        List<Topic> ListActiveTopicByCourseId(Guid courseId);
    }
}

