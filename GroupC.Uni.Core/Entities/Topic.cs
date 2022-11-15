using GroupC.Uni.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GroupC.Uni.Core.Entities
{
    [DisplayColumn("Name")]
    public class Topic : BaseEntity, IAggregateRoot
    {
        public Topic() : base()
        {
            Questions = new HashSet<Question>();
        }
        
        public string Name { get; set; }

        //[ForeignKey("Course")]
        public Guid CourseId { get; set; }
        public Course Course { get; set; }

        public virtual ICollection<Question> Questions { get; set; }


    }
}
