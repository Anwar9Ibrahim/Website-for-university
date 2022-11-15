using GroupC.Uni.Core.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GroupC.Uni.Web.ViewModels
{
    public class CourseModelView
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "The Name field is required.")]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "The Code field is required.")]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public IFormFile Image { get; set; }
        [Display(Name = "Image URL")]
        public string ImageURL { get; set; }
        public MyEnums.status Status { get; set; }
        public virtual ICollection<Exam> Exams { get; set; }
        // public virtual ICollection<Topic> Topics { get; set; }

        public List<CreateTopicViewModel> Topics { get; set; }
    }


    public class CreateTopicViewModel
    {
        [Required(ErrorMessage = "The Topic name field is required.")]
        [Display(Name = "Topic Name")]
        public string TopicName { get; set; }
    }
}
