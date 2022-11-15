using GroupC.Uni.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GroupC.Uni.Web.ViewModels
{
    public class QuestionViewModel
    {
        public Guid Id { get; set; }
        [Required]
        public string Text { get; set; }

        [Range(1, 100, ErrorMessage = "Value must be between 1 to 100")]
        public double Mark { get; set; }
        [Display(Name = "Is Html")]
        public bool IsHtml { get; set; }
        public Guid TopicId { get; set; }
        public Topic Topic { get; set; }
        public MyEnums.status Status { get; set; }

       
        public List<CreateChoiceViewModel> Choices { get; set; }
    }

    public class CreateChoiceViewModel
    {
        [Display(Name = "Is Correct")]
        public bool Type { get; set; }
        [Required]
        public string Text { get; set; }
    }

}
