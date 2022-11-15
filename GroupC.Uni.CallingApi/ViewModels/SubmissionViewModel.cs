using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GroupC.Uni.Core.Entities;
using GroupC.Uni.Web.ViewModels;
using Microsoft.AspNetCore.Http;

namespace GroupC.Uni.ConsumingApi.ViewModels
{
    public class SubmissionViewModel
    {
        public Guid Id { get; set; }
        [Display(Name = "Submission Choices")]
        public ICollection<SubmissionChoiceViewModel> SubmissionChoices { get; set; }
        [Display(Name = "Submission Date")]
        public DateTime date { get; set; }
        [Display(Name = "Student Id")]
        public Guid StudentId { get; set; }
        [Display(Name = "Student Name")]
        public GroupC.Uni.ConsumingApi.ViewModels.StudentViewModel Student { get; set; }
        [Display(Name = "Exam Id")]
        public Guid ExamId { get; set; }
        [Display(Name = "Exam Course")]
        public GroupC.Uni.ConsumingApi.ViewModels.ExamViewModel Exam { get; set; }
    }
    public class StudentViewModel
    {
        public MyEnums.UserType UserType = MyEnums.UserType.Student;
        [Required]
        [Range(1, 6)]
        public int Year { get; set; }
        public ApplicationUserViewModel ApplicationUser { get; set; }
        public Guid ApplicationUserId { get; set; }
    }
    public class SubmissionChoiceViewModel
    {
        public Guid Id { get; set; }
        [Display(Name = "Choice Id")]
        public Guid ChoiceId { get; set; }
        [Display(Name = "Choice")]
        public ChoiceViewModel Choice { get; set; }
        [Display(Name = "Submission Id")]
        public Guid SubmissionId { get; set; }
        [Display(Name = "Submission")]
        public SubmissionViewModel Submission { get; set; }

    }
    public class ChoiceViewModel
    {
        public Guid Id { get; set; }
        [Display(Name = "Is Correct")]
        public bool Type { get; set; }
        [Required]
        public string Text { get; set; }
        [Display(Name = "QuestionId")]
        public Guid QuestionId { get; set; }
        [Display(Name = "Question")]
        public QuestionViewModel Question { get; set; }
       
    }
    public class ExamViewModel
    {
        public Guid Id { get; set; }
        [Required]
        //  [ValidateDateRange(FirstDate = Convert.ToDateTime("01/10/2008"), SecondDate = Convert.ToDateTime("01/12/2008"))]
        [Display(Name = "Duration In Mins")]
        public int DurationInMinutes { get; set; }
        //public bool IsRandom { get; set; }
        [Display(Name = "Question Count")]
        public int QuestionsCount { get; set; }
        [Display(Name = "Course")]
        public Guid CourseId { get; set; }
        [Display(Name = "Course")]
        public CourseModelView Course { get; set; }
        [Display(Name = "Test Center")]
        public Guid TestCenterId { get; set; }
        //[Display(Name = "Test Center")]
        //public TestCenterExamViewModel TestCenter { get; set; }
        [Display(Name = "Exam Date")]

        public DateTime ExamDateTime { get; set; }
        public ICollection<ExamQuestion> ExamQuestions { get; set; }
        public ICollection<QuestionIndexViewModel> Questions { get; set; }
    }
    public class ApplicationUserViewModel
    {
        public Guid Id { get; set; }
        public string userName { get; set; }
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
        public StudentViewModel studentViewModel { get; set; }
        public TestCenterViewModel testCenterViewModel { get; set; }
        public MyEnums.UserType UserType { get; set; }
        // [Range(1,6)]
        public int Year { get; set; }
        public IFormFile Image { get; set; }
        public string ImageURL { get; set; }
    }
}


