using System;
using System.Collections.Generic;
using System.Text;
using GroupC.Uni.Core.Interfaces;

namespace GroupC.Uni.Core.Entities
{
    public class ExamQuestion : BaseEntity, IAggregateRoot
    {

        public int Order { get; set; }
        public double Mark { get; set; }
        public Guid ExamId { get; set; }
        public Guid QuestionId { get; set; }
        public virtual Exam Exam { get; set; }
        public virtual Question Question { get; set; }
    }
}
