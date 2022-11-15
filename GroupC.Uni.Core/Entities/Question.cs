using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using GroupC.Uni.Core.Interfaces;

namespace GroupC.Uni.Core.Entities
{
    
    public class Question:BaseEntity, IAggregateRoot
    {
        public Question():base()
        {
            Choices = new HashSet<Choice>();
            ExamQuestions = new HashSet<ExamQuestion>();
        }
        public string Text { get; set; }
		public double Mark { get; set; }
        [DisplayFormat(DataFormatString ="Is Html")]
		public bool IsHtml { get; set; }
        public Guid TopicId { get; set; }
        public virtual Topic Topic { get; set; }
        public virtual ICollection<Choice> Choices { get; set; }
        public virtual ICollection<ExamQuestion> ExamQuestions { get; set; }
    }
}
