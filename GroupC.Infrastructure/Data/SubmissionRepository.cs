using GroupC.Uni.Core.Entities;
using GroupC.Uni.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroupC.Uni.Infrastructure.Data
{
    public class SubmissionRepository : EfRepository<Submission>, ISubmissionRepository
    {
        public SubmissionRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Submission> getByIdWithAll(Guid id)
        {
            var result = await _dbContext.Submissions.Where(i => i.Id == id).FirstOrDefaultAsync();
            ICollection<SubmissionChoice> _SubmissionChoices = await _dbContext.SubmissionChoices.
               Include(q => q.Choice)
               .Where(q => q.Status == MyEnums.status.Active)
               .Where(q => q.SubmissionId == result.Id)
               .Select(q => new SubmissionChoice()
               {
                   Id = q.Id,
                   SubmissionId = q.SubmissionId,
                   ChoiceId = q.ChoiceId,
                   Choice = new Choice()
                   {
                       Id = q.Choice.Id,
                       QuestionId = q.Choice.QuestionId,
                       Text = q.Choice.Text,
                       Question = new Question()
                       {
                           Text = q.Choice.Question.Text,
                           Topic = q.Choice.Question.Topic,
                           IsHtml = q.Choice.Question.IsHtml,
                           Choices = q.Choice.Question.Choices
                       }
                   },

               }).ToListAsync();
            var questions = await _dbContext.ExamQuestions
                .Include(examQuestion => examQuestion.Question)
              .Where(examQuestion => examQuestion.ExamId == result.ExamId)
              .Where(examQuestion => examQuestion.Status == MyEnums.status.Active)
              .Select(examQuestion => new ExamQuestion
              {
                  Order = examQuestion.Order,
                  Mark = examQuestion.Mark,
                  ExamId = examQuestion.ExamId,
                  QuestionId = examQuestion.QuestionId,
                  Question = new Question
                  {
                      Text = examQuestion.Question.Text,
                      Topic = examQuestion.Question.Topic,
                      IsHtml = examQuestion.Question.IsHtml,
                      Choices = examQuestion.Question.Choices
                  }
              }).ToListAsync();
            var results = await _dbContext.Submissions
                   .Include(q => q.Exam)
                   .Include(q => q.Exam.Course)
                   .Include(q => q.Exam.TestCenter)
                   .Include(q => q.Exam.TestCenter.ApplicationUser)
                   .Include(q => q.Exam.ExamQuestions)
                   .Include(q => q.Student)
                   .Include(q => q.Student.ApplicationUser)
                   .Include(q => q.SubmissionChoices)
                     .ThenInclude(it => it.Choice)
                   .Where(q => q.Status == MyEnums.status.Active)
                   .Where(q => q.Id == id)
                   .Select(q => new Submission
                   {
                       Id = q.Id,
                       StudentId = q.StudentId,
                       ExamId = q.ExamId,
                       date = q.date,
                       Exam = new Exam
                       {
                           Id = q.Exam.Id,
                           CourseId = q.Exam.CourseId,
                           DurationInMinutes = q.Exam.DurationInMinutes,
                           IsRandom = q.Exam.IsRandom,
                           QuestionsCount = q.Exam.QuestionsCount,
                           TestCenterId = q.Exam.TestCenterId,
                           Course = new Course
                           {
                               Name = q.Exam.Course.Name,
                               Code = q.Exam.Course.Code
                           },
                           TestCenter = new TestCenter
                           {
                               Name = q.Exam.TestCenter.ApplicationUser.UserName,
                               ApplicationUserId = q.Exam.TestCenter.ApplicationUserId
                           },
                           ExamQuestions=questions
                       },
                       Student = new Student()
                       {
                           Id = q.Student.Id,
                           Year = q.Student.Year,
                           ApplicationUserId = q.Student.ApplicationUserId,
                           Submissions = q.Student.Submissions,
                           userType = q.Student.userType,
                          
                           ApplicationUser = new ApplicationUser()
                           {Email=q.Student.ApplicationUser.Email,
                           UserName=q.Student.ApplicationUser.UserName,
                           ImageURL=q.Student.ApplicationUser.ImageURL
                           }
                       },
                       SubmissionChoices = _SubmissionChoices
                   }).FirstOrDefaultAsync(); 
            return results;
        }

        public async Task<IReadOnlyList<Submission>> ListActiveSyncWithAll()
        {
            ICollection<SubmissionChoice> _SubmissionChoices = await _dbContext.SubmissionChoices.
               Include(q => q.Choice)
               .Where(q => q.Status == MyEnums.status.Active)
               .Select(q => new SubmissionChoice()
               {
                   Id = q.Id,
                   SubmissionId = q.SubmissionId,
                   ChoiceId = q.ChoiceId,
                   Choice = new Choice()
                   {
                       Id = q.Choice.Id,
                       QuestionId = q.Choice.QuestionId,
                       Text = q.Choice.Text,
                       Question = new Question()
                       {
                           Text = q.Choice.Question.Text,
                           Topic = q.Choice.Question.Topic,
                           IsHtml = q.Choice.Question.IsHtml,
                           Choices = q.Choice.Question.Choices
                       }
                   },

               }).ToListAsync();
            var results = await _dbContext.Submissions
                   .Include(q => q.Exam)
                   .Include(q => q.Exam.Course)
                   .Include(q => q.Exam.TestCenter)
                   .Include(q => q.Exam.TestCenter.ApplicationUser)
                   .Include(q => q.Exam.ExamQuestions)
                   .Include(q => q.Student)
                   .Include(q => q.Student.ApplicationUser)
                   .Include(q => q.SubmissionChoices)
                     .ThenInclude(it => it.Choice)
                   .Where(q => q.Status == MyEnums.status.Active)
                   .Select(q => new Submission
                   {
                       Id = q.Id,
                       StudentId = q.StudentId,
                       ExamId = q.ExamId,
                       date = q.date,
                       Exam = new Exam
                       {
                           Id = q.Exam.Id,
                           CourseId = q.Exam.CourseId,
                           DurationInMinutes = q.Exam.DurationInMinutes,
                           IsRandom = q.Exam.IsRandom,
                           QuestionsCount = q.Exam.QuestionsCount,
                           TestCenterId = q.Exam.TestCenterId,
                           Course = new Course
                           {
                               Name = q.Exam.Course.Name,
                               Code = q.Exam.Course.Code
                           },
                           TestCenter = new TestCenter
                           {
                               Name = q.Exam.TestCenter.ApplicationUser.UserName,
                               ApplicationUserId = q.Exam.TestCenter.ApplicationUserId
                           }
                       },
                       Student = new Student()
                       {
                           Id = q.Student.Id,
                           Year = q.Student.Year,
                           ApplicationUserId = q.Student.ApplicationUserId,
                           Submissions = q.Student.Submissions,
                           userType = q.Student.userType,
                            ApplicationUser = new ApplicationUser()
                            {
                                Email = q.Student.ApplicationUser.Email,
                                UserName = q.Student.ApplicationUser.UserName,
                                ImageURL = q.Student.ApplicationUser.ImageURL
                            }
                       },
                       SubmissionChoices = _SubmissionChoices.Where(p => p.SubmissionId == q.Id).ToList()
                   }).ToListAsync();
            return results;

        }

        public async Task<List<Submission>> listSubmissionsByExamId(Guid id)
        {
            ICollection<SubmissionChoice> _SubmissionChoices = await _dbContext.SubmissionChoices.
               Include(q => q.Choice)
               .Where(q => q.Status == MyEnums.status.Active)
               .Where(q => q.Submission.ExamId == id)
               .Select(q => new SubmissionChoice()
               {
                   Id = q.Id,
                   SubmissionId = q.SubmissionId,
                   ChoiceId = q.ChoiceId,
                   Choice = new Choice()
                   {
                       Id = q.Choice.Id,
                       QuestionId = q.Choice.QuestionId,
                       Text = q.Choice.Text,
                       Question = new Question()
                       {
                           Text = q.Choice.Question.Text,
                           Topic = q.Choice.Question.Topic,
                           IsHtml = q.Choice.Question.IsHtml,
                           Choices = q.Choice.Question.Choices
                       }
                   },

               }).ToListAsync();
            var results = await _dbContext.Submissions
                   .Include(q => q.Exam)
                   .Include(q => q.Exam.Course)
                   .Include(q => q.Exam.TestCenter)
                   .Include(q => q.Exam.TestCenter.ApplicationUser)
                   .Include(q => q.Exam.ExamQuestions)
                   .Include(q => q.Student)
                   .Include(q => q.Student.ApplicationUser)
                   .Include(q => q.SubmissionChoices)
                     .ThenInclude(it => it.Choice)
                   .Where(q => q.Status == MyEnums.status.Active)
                   .Where(q => q.ExamId == id)
                   .Select(q => new Submission
                   {
                       Id = q.Id,
                       StudentId = q.StudentId,
                       ExamId = q.ExamId,
                       date = q.date,
                       Exam = new Exam
                       {
                           Id = q.Exam.Id,
                           CourseId = q.Exam.CourseId,
                           DurationInMinutes = q.Exam.DurationInMinutes,
                           IsRandom = q.Exam.IsRandom,
                           QuestionsCount = q.Exam.QuestionsCount,
                           TestCenterId = q.Exam.TestCenterId,
                           Course = new Course
                           {
                               Name = q.Exam.Course.Name,
                               Code = q.Exam.Course.Code
                           },
                           TestCenter = new TestCenter
                           {
                               Name = q.Exam.TestCenter.ApplicationUser.UserName,
                               ApplicationUserId = q.Exam.TestCenter.ApplicationUserId
                           }
                       },
                       Student = new Student()
                       {
                           Id = q.Student.Id,
                           Year = q.Student.Year,
                           ApplicationUserId = q.Student.ApplicationUserId,
                           Submissions = q.Student.Submissions,
                           userType = q.Student.userType,
                            ApplicationUser = new ApplicationUser()
                            {
                                Email = q.Student.ApplicationUser.Email,
                                UserName = q.Student.ApplicationUser.UserName,
                                ImageURL = q.Student.ApplicationUser.ImageURL
                            }
                       },
                       SubmissionChoices = _SubmissionChoices.Where(p => p.SubmissionId == q.Id).ToList()
                   }).ToListAsync();
            return results;
        }

        public async Task<List<Submission>> listSubmissionsByStudentId(Guid id)
        {

            ICollection<SubmissionChoice> _SubmissionChoices = await _dbContext.SubmissionChoices.
             Include(q => q.Choice)
             .Where(q => q.Status == MyEnums.status.Active)
             .Where(s => s.Submission.StudentId == id)
             .Select(q => new SubmissionChoice()
             {
                 Id = q.Id,
                 SubmissionId = q.SubmissionId,
                 ChoiceId = q.ChoiceId,
                 Choice = new Choice()
                 {
                     Id = q.Choice.Id,
                     QuestionId = q.Choice.QuestionId,
                     Text = q.Choice.Text,
                     Question = new Question()
                     {
                         Text = q.Choice.Question.Text,
                         Topic = q.Choice.Question.Topic,
                         IsHtml = q.Choice.Question.IsHtml,
                         Choices = q.Choice.Question.Choices
                     }
                 },

             }).ToListAsync();
            var results = await _dbContext.Submissions
                   .Include(q => q.Exam)
                   .Include(q => q.Exam.Course)
                   .Include(q => q.Exam.TestCenter)
                   .Include(q => q.Exam.TestCenter.ApplicationUser)
                   .Include(q => q.Exam.ExamQuestions)
                   .Include(q => q.Student)
                   .Include(q => q.Student.ApplicationUser)
                   .Include(q => q.SubmissionChoices)
                     .ThenInclude(it => it.Choice)
                   .Where(q => q.Status == MyEnums.status.Active)
                   .Where(s => s.StudentId == id)
                   .Select(q => new Submission
                   {
                       Id = q.Id,
                       StudentId = q.StudentId,
                       ExamId = q.ExamId,
                       date = q.date,
                       Exam = new Exam
                       {
                           Id = q.Exam.Id,
                           CourseId = q.Exam.CourseId,
                           DurationInMinutes = q.Exam.DurationInMinutes,
                           IsRandom = q.Exam.IsRandom,
                           QuestionsCount = q.Exam.QuestionsCount,
                           TestCenterId = q.Exam.TestCenterId,
                           Course = new Course
                           {
                               Name = q.Exam.Course.Name,
                               Code = q.Exam.Course.Code
                           },
                           TestCenter = new TestCenter
                           {
                               Name = q.Exam.TestCenter.ApplicationUser.UserName,
                               ApplicationUserId = q.Exam.TestCenter.ApplicationUserId
                           }
                       },
                       Student = new Student()
                       {
                           Id = q.Student.Id,
                           Year = q.Student.Year,
                           ApplicationUserId = q.Student.ApplicationUserId,
                           Submissions = q.Student.Submissions,
                           userType = q.Student.userType,
                            ApplicationUser = new ApplicationUser()
                            {
                                Email = q.Student.ApplicationUser.Email,
                                UserName = q.Student.ApplicationUser.UserName,
                                ImageURL = q.Student.ApplicationUser.ImageURL
                            }
                       },
                       SubmissionChoices = _SubmissionChoices.Where(p => p.SubmissionId == q.Id).ToList()
                   }).ToListAsync();
            return results;
        }

        public new async Task<IEnumerable<Submission>> ListAllAsyncNoReadOnly()
        {
            ICollection<SubmissionChoice> _SubmissionChoices = await _dbContext.SubmissionChoices.
                Include(q => q.Choice)
                .Where(q => q.Status == MyEnums.status.Active)
                .Select(q => new SubmissionChoice()
                {
                    Id = q.Id,
                    SubmissionId = q.SubmissionId,
                    ChoiceId = q.ChoiceId,
                    Choice = new Choice()
                    {
                        Id = q.Choice.Id,
                        QuestionId = q.Choice.QuestionId,
                        Text = q.Choice.Text,
                        Question = new Question()
                        {
                            Text = q.Choice.Question.Text,
                            Topic = q.Choice.Question.Topic,
                            IsHtml = q.Choice.Question.IsHtml,
                            Choices = q.Choice.Question.Choices
                        }
                    },

                }).ToListAsync();
            var results = await _dbContext.Submissions
                   .Include(q => q.Exam)
                   .Include(q => q.Exam.Course)
                   .Include(q => q.Exam.TestCenter)
                   .Include(q => q.Exam.TestCenter.ApplicationUser)
                   .Include(q => q.Exam.ExamQuestions)
                   .Include(q => q.Student)
                   .Include(q => q.Student.ApplicationUser)
                   .Include(q => q.SubmissionChoices)
                     .ThenInclude(it => it.Choice)
                   .Where(q => q.Status == MyEnums.status.Active)
                   .Select(q => new Submission
                   {
                       Id = q.Id,
                       StudentId = q.StudentId,
                       ExamId = q.ExamId,
                       date = q.date,
                       Exam = new Exam
                       {
                           Id = q.Exam.Id,
                           CourseId = q.Exam.CourseId,
                           DurationInMinutes = q.Exam.DurationInMinutes,
                           IsRandom = q.Exam.IsRandom,
                           QuestionsCount = q.Exam.QuestionsCount,
                           TestCenterId = q.Exam.TestCenterId,
                           Course = new Course
                           {
                               Name = q.Exam.Course.Name,
                               Code = q.Exam.Course.Code
                           },
                           TestCenter = new TestCenter
                           {
                               Name = q.Exam.TestCenter.ApplicationUser.UserName,
                               ApplicationUserId = q.Exam.TestCenter.ApplicationUserId
                           }
                       },
                       Student = new Student()
                       {
                           Id = q.Student.Id,
                           Year = q.Student.Year,
                           ApplicationUserId = q.Student.ApplicationUserId,
                           Submissions = q.Student.Submissions,
                           userType = q.Student.userType,
                            ApplicationUser = new ApplicationUser()
                            {
                                Email = q.Student.ApplicationUser.Email,
                                UserName = q.Student.ApplicationUser.UserName,
                                ImageURL = q.Student.ApplicationUser.ImageURL
                            }
                       },
                       SubmissionChoices = _SubmissionChoices.Where(p => p.SubmissionId == q.Id).ToList()
                   }).ToListAsync();
            return results;
        }
    }
}
