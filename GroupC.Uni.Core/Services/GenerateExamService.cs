using GroupC.Uni.Core.Entities;
using GroupC.Uni.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GroupC.Uni.Core.Services
{
    public class GenerateExamService : IGenerateExamService
    {
        private readonly IExamRepository _examRepositry;
        private readonly IQuestionRepository _questionRepositry;
        private readonly ITopicService _topicRepository;
        private readonly IAppLogger<IGenerateExamService> _logger;

        public GenerateExamService(IExamRepository examRepositry,
            IQuestionRepository questionRepositry,
            ITopicService topicR, 
            IAppLogger<IGenerateExamService> logger) 
        {
            _examRepositry = examRepositry;
             _topicRepository = topicR;
            _questionRepositry = questionRepositry;
            _logger = logger;
        }
    
        public List<ExamQuestion> GenerateExam(int numOfQuestions,Guid courseId)
        {
            List<Question> ChoosenQuestions = new List<Question>(); ;
            int index_1;
            int index;
            int numOfAvailableQuestion=0;
            List<Topic> topics = _topicRepository.ListActiveTopicByCourseId(courseId);
            List<List<Question>> topic_ques = new List<List<Question>>();
            List<Question> q;
            foreach (Topic topic in topics)
            {
                q = _questionRepositry.GetByTopicIdWithTopic(topic.Id);
                topic_ques.Add(q);
                numOfAvailableQuestion += q.Count;
            }
            if (numOfQuestions > numOfAvailableQuestion)
                return null;
            //if (numOfQuestions >= topics.Count)
            //{
            //    for (int i = 1; i < numOfQuestions+1; i++)
            //    {
            //            var random = new Random();
            //        index = random.Next(topic_ques[(numOfQuestions%i)].Count);

            //        //if(!ChoosenQuestions.Contains(topic_ques[(numOfQuestions % i)][index]))
            //            ChoosenQuestions.Add(topic_ques[(numOfQuestions % i)][index]);
            //    }

            //}
            //else
            double sum = 0;
                for (int i = 1; i < numOfQuestions+1; i++)
                {
                    var random = new Random();
                    index = random.Next(topic_ques.Count);
                    var random_2 = new Random();
                    index_1 = random_2.Next(topic_ques[index].Count);
                if (!ChoosenQuestions.Contains(topic_ques[index][index_1]))
                {
                    ChoosenQuestions.Add(topic_ques[index][index_1]);
                    sum += topic_ques[index][index_1].Mark;
                }
                else
                    i = i - 1;
                }
            ChoosenQuestions.OrderByDescending(o => o.Mark).ToList();
            int totalMark=0;
            List<ExamQuestion> final = new List<ExamQuestion>();
            int ind = 1;
            foreach (Question ques in ChoosenQuestions)
            {
                final.Add(new ExamQuestion() {
                    Question = ques,
                    Order = ind,
                    Mark = (int)((ques.Mark / sum) * 100),
                    QuestionId=ques.Id
                    
                });
                totalMark+= (int)((ques.Mark / sum) * 100);

            }
            final.Find(x => x.Order == 1).Mark += (100 - totalMark);
            return final;
        }
    }
}
