using GroupC.Uni.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GroupC.Uni.Web.ViewModels
{
    public class ChoiceViewModels
    {
        public Guid Id { get; set; }
        [Display(Name ="Is Correct")]
        public bool Type { get; set; }
        [Required]
        public string Text { get; set; }
        public Guid QuestionId { get; set; }
        public virtual Question Question { get; set; }
        public virtual ICollection<SubmissionChoice> SubmissionChoices { get; set; }
        public MyEnums.status Status { get; set; }
    }
}
