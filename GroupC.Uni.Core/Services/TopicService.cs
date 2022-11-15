using GroupC.Uni.Core.Entities;
using GroupC.Uni.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GroupC.Uni.Core.Services
{
    public class TopicService: Service<Topic>,ITopicService
    {
        private readonly ITopicRepository _topicRepositry;
        private readonly IAppLogger<ITopicService> _logger;

        public TopicService(ITopicRepository topicRepositry, IAppLogger<ITopicService> logger):base(topicRepositry)
        {
            _topicRepositry = topicRepositry;
            _logger = logger;
        }

        public async Task CreateTopicAsync(Topic _Topic)
        {
            await _topicRepositry.AddAsync(_Topic);
        }

        //public async Task<List<string>> ListTopicNamesAsync()
        //{

        //    var topics = await _topicRepositry.ListAllAsync();
        //    List<string> topicsName = new List<String>();

        //    foreach (var topic in topics)
        //    {
        //        topicsName.Add(topic.Name);
        //    }
        //    return topicsName;
        //}
        

        public async Task<Topic> GetByIdWithCourse(Guid id)
        {
            return await _topicRepositry.GetByIdWithCourse(id);
        }

        public async Task<IReadOnlyList<Topic>> ListActiveSyncWithCourse()
        {
            return await _topicRepositry.ListActiveSyncWithCourse();
        }

       

        public async Task SpecialUpdateAsync(Topic _Topic)
        {
            await _topicRepositry.SpecialUpdateAsync(_Topic);
        }
        public List<Topic> ListActiveTopicByCourseId(Guid courseId)
        {
            return  _topicRepositry.ListActiveTopicByCourseId(courseId);
        }
    }
}
