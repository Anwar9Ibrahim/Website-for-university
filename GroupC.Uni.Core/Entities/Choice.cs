using System;
using System.Collections.Generic;
using System.Text;
using GroupC.Uni.Core.Interfaces;

namespace GroupC.Uni.Core.Entities
{
	public class Choice :BaseEntity, IAggregateRoot
    {
        public Choice() :base()
        {
            SubmissionChoices = new HashSet<SubmissionChoice>();
        }
        public string Text { get; set; }
        public bool Type { get; set; }
        public Guid QuestionId { get; set; }
        public virtual Question Question { get; set; }
        public virtual ICollection<SubmissionChoice> SubmissionChoices { get; set; }
    }
}
