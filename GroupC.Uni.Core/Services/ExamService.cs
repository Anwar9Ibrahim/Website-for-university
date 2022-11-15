using System;
using System.Collections.Generic;
using System.Text;
using GroupC.Uni.Core.Interfaces;
//using GroupC.Uni.Core.Specifications;
using GroupC.Uni.Core.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace GroupC.Uni.Core.Services
{
    public class ExamService : Service<Exam>, IExamService
    {
        private readonly IExamRepository _examRepository;
        private readonly ITopicRepository _topicRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IChoiceRepository _choicenRepository;
        private readonly IAppLogger<ExamService> _logger;

        public ExamService(IExamRepository examRepository,
            ITopicRepository topicRepository,
            IQuestionRepository questionRepository,
            IChoiceRepository choicenRepository,
            IAppLogger<ExamService> logger) : base(examRepository)
        {
            _examRepository = examRepository;
            _topicRepository = topicRepository;
            _questionRepository = questionRepository;
            _choicenRepository = choicenRepository;
            _logger = logger;
        }
        public async Task createExam(Guid CourseId, int questionNum = 1)
        {
           //List<Topic>currentTopics= await _topicRepository.ListTopicByCourseId(CourseId);
           // foreach (Topic currentTopic in currentTopics)
           // {
           //     List<Question> currentQuestions = await _questionRepository.ListQuestionByTopicId(currentTopic.Id);
           //   //  List<Question> SortedQuestions = objListOrder.OrderBy(o => o.).ToList();
           // }
           

        }
        public async Task<IReadOnlyList<Exam>> ListAllAsyncWithExTT() {
            return await _examRepository.ListAllAsyncWithExTT();
        }
        
        public async Task specialUpdateAsync(Exam exam)
        {
            await _examRepository.specialUpdateAsync(exam);
        }
        public async Task<Exam> GetExamWithAttributes(Guid id)
        {
            return await _examRepository.GetExamWithAttributes(id);
        }
        public async Task<IReadOnlyList<Exam>> GetLatestExamsAsync()
        {
            return await _examRepository.GetLatestExamsAsync();
        }
        public async Task<IReadOnlyList<Exam>> ListAllAsyncWithExams()
        {
            return await _examRepository.ListAllAsyncWithExams();
        }
        public async Task<Exam> GetExamAsyncwithQuestions(Guid guid)
        {
            return await _examRepository.GetExamAsyncwithQuestions(guid);
        }
    }
}