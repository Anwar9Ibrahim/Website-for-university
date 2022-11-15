using GroupC.Uni.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GroupC.Uni.Web.ViewModels
{
    public class QuestionIndexViewModel
    {
        public Guid Id { get; set; }
        [Required]
        public string Text { get; set; }

        [Range(1, 100, ErrorMessage = "Value must be between 1 to 100")]
        public double Mark { get; set; }
        [Display(Name = "Is Html")]
        public bool IsHtml { get; set; }
        public Guid TopicId { get; set; }
        [Display(Name = "Topic")]
        public String TopicName { get; set; }
        public MyEnums.status Status { get; set; }

        public List<ChoiceViewModels> Choices { get; set; }
        public string choices_string { get; set; }
    }

   

}
