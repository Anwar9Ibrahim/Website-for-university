using GroupC.Uni.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GroupC.Uni.Web.ViewModels
{
    public class TopicViewModels
    {
        [Required]
        public string Name { get; set; }
        public Guid Id { get; set; }
        //[ForeignKey("Course")]

        public Guid CourseId { get; set; }
        public Course Course { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
        public MyEnums.status Status { get; set; }

        
    }


}
