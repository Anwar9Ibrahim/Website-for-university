using GroupC.Uni.Core.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GroupC.Uni.Web.ViewModels
{
        public class ExamViewModel 
        {
        public Guid Id { get; set; }
        [Display(Name = "Duration In Mins")]
        public int DurationInMinutes { get; set; }
        //public bool IsRandom { get; set; }
        [Display(Name = "Question Count")]
        public int QuestionsCount { get; set; }
        [Display(Name = "Course")]
        public Guid CourseId { get; set; }
        [Display(Name = "Course")]
        public  CourseModelView CourseViewModel { get; set; }
        [Display(Name = "Test Center")]
        public Guid TestCenterId { get; set; }
        [Display(Name = "Test Center")]
        public TestCenterExamViewModel TestCenterViewModel { get; set; }
        [Display(Name = "Exam Date")]
        public DateTime ExamDateTime { get; set; }
        public  ICollection<ExamQuestionViewModel> ExamQuestionsViewModel { get; set; }
        public ICollection<QuestionIndexViewModel> QuestionsViewModel { get; set; }


        //public  ICollection<ExamQuestion> ExamQuestions { get; set; }

        //public  ICollection<Submission> Submissions { get; set; }

    }

    public class ExamQuestionViewModel
    {
        public int Order { get; set; }
        public double Mark { get; set; }
        public Guid ExamId { get; set; }
        public Guid QuestionId { get; set; }
        public  ExamViewModel Exam { get; set; }
        public  QuestionIndexViewModel Question { get; set; }
    }
    public class TestCenterExamViewModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }


        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        public MyEnums.UserType UserType { get; set; }
        public IFormFile Image { get; set; }

        public string ImageURL { get; set; }
    }
}
