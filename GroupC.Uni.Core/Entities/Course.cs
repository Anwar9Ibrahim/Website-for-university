using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using GroupC.Uni.Core.Interfaces;

namespace GroupC.Uni.Core.Entities
{
    [DisplayColumn("Name")]
    public class Course:BaseEntity, IAggregateRoot
    {
        public Course():base()
        {
            Exams = new HashSet<Exam>();
            Topics = new HashSet<Topic>();
        }
        
		public string Name { get; set; }
        public string Code { get; set; }
		public string ImageURL { get; set; }
        public virtual ICollection<Exam> Exams { get; set; }
        public virtual ICollection<Topic> Topics { get; set; }
    }
}
