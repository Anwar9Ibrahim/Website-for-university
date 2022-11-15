using GroupC.Uni.Core.Entities;
using GroupC.Uni.Core.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupC.Uni.Infrastructure.Data
{
    public class ExamRepository : EfRepository<Exam>, IExamRepository
    {
        public ExamRepository(AppDbContext dbContext) : base(dbContext)
        {

        }
        public async Task<IReadOnlyList<Exam>> ListAllAsyncWithExTT()
        {
            return await _dbContext.Exams.Include(q => q.ExamQuestions).Include(q => q.Course).Include(q => q.TestCenter.ApplicationUser).Where(q => q.Status == MyEnums.status.Active).ToListAsync();
        }

        public async Task specialUpdateAsync(Exam exam)
        {
            Exam _exam = await _dbContext.Set<Exam>().FindAsync(exam.Id);
            //_exam.Course = exam.Course;
            //_exam.CourseId = exam.CourseId;
            _exam.DurationInMinutes = exam.DurationInMinutes;
            _exam.ExamDate = exam.ExamDate;
            //_exam.ExamQuestions = new List<ExamQuestion>();
            //_exam.ExamQuestions = exam.ExamQuestions;
            //_exam.IsRandom = exam.IsRandom;
            //_exam.QuestionsCount = exam.QuestionsCount;
            //_exam.Status = exam.Status;
            //_exam.Submissions = exam.Submissions;
            //_exam.TestCenter = exam.TestCenter;
            //_exam.TestCenterId = exam.TestCenterId;
            _exam.LastUpdateDate = System.DateTime.Now;
            await _dbContext.SaveChangesAsync();
        }
        public async Task<Exam> GetExamWithAttributes(Guid id)
        {
            return await _dbContext.Exams
               .Include(e => e.Course)
               .Include(e => e.TestCenter).Include(e => e.TestCenter.ApplicationUser).Include(e => e.ExamQuestions)
               .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IReadOnlyList<Exam>> GetLatestExamsAsync()
        {
            return await _dbContext.Exams.Where(q => q.Status == MyEnums.status.Active).OrderByDescending(d => d.ExamDate).Take(5).ToListAsync();

        }
        public async Task<IReadOnlyList<Exam>> ListAllAsyncWithExams()
        {
           
            var result = await _dbContext.Exams.Include(exam => exam.Course)
                .Include(exam => exam.TestCenter.ApplicationUser)
                .Include(exam => exam.ExamQuestions)
                .Where(exam => exam.Status == MyEnums.status.Active)
                .Select(exam => new Exam
                {
                    Id = exam.Id,
                    CourseId = exam.CourseId,
                    DurationInMinutes = exam.DurationInMinutes,
                    IsRandom = exam.IsRandom,
                    QuestionsCount = exam.QuestionsCount,
                    TestCenterId = exam.TestCenterId,
                    Course = new Course
                    {
                        Name = exam.Course.Name,
                        Code = exam.Course.Code
                    },
                    TestCenter = new TestCenter
                    {
                        Name= exam.TestCenter.ApplicationUser.UserName,                        
                        ApplicationUserId= exam.TestCenter.ApplicationUserId
                    }
                }).ToListAsync();
            return result;
        }
        public async Task<Exam> GetExamAsyncwithQuestions(Guid guid)
        {
            var questions = await _dbContext.ExamQuestions.Include(examQuestion => examQuestion.Question)
                .Where(examQuestion => examQuestion.ExamId == guid)
                .Where(examQuestion => examQuestion.Status == MyEnums.status.Active)
                .Select(examQuestion => new ExamQuestion
                {
                    Order= examQuestion.Order,
                    Mark = examQuestion.Mark,
                    ExamId= examQuestion.ExamId,
                    QuestionId= examQuestion.QuestionId,
                    Question = new Question
                    {
                        Text = examQuestion.Question.Text,
                        Topic = examQuestion.Question.Topic,
                        IsHtml = examQuestion.Question.IsHtml,
                        Choices = examQuestion.Question.Choices
                    }
                }).ToListAsync();
            var result = await _dbContext.Exams.Include(exam => exam.Course)
                 .Include(exam => exam.TestCenter.ApplicationUser)
                 .Include(exam => exam.ExamQuestions)
                 .ThenInclude(ExamQuestions => ExamQuestions.Question)
                 .Where(exam => exam.Status == MyEnums.status.Active)
                 .Where(exam => exam.Id == guid)
                 .Select(exam => new Exam
                 {
                     Id = exam.Id,
                     CourseId = exam.CourseId,
                     DurationInMinutes = exam.DurationInMinutes,
                     IsRandom = exam.IsRandom,
                     QuestionsCount = exam.QuestionsCount,
                     TestCenterId = exam.TestCenterId,
                     Course = new Course
                     {
                         Name = exam.Course.Name,
                         Code = exam.Course.Code
                     },
                     TestCenter = new TestCenter
                     {
                         Name = exam.TestCenter.ApplicationUser.UserName,
                         ApplicationUserId = exam.TestCenter.ApplicationUserId
                     },
                     ExamQuestions = questions,
                     ExamDate = exam.ExamDate
                 })
                 .FirstOrDefaultAsync();
            return result;
        }
    }
}

