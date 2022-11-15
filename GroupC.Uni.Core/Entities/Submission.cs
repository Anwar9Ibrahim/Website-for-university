using GroupC.Uni.Core.Interfaces;
using System;
using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GroupC.Uni.Core.Entities
{
    public class Submission : BaseEntity, IAggregateRoot
    {
        public Submission() : base()
        {
            SubmissionChoices = new HashSet<SubmissionChoice>();
        }
        public virtual ICollection<SubmissionChoice> SubmissionChoices { get; set; }

        public DateTime date { get; set; }


        //[ForeignKey("Student")]
        public Guid StudentId { get; set; }
        public Student Student { get; set; }

        // [ForeignKey("Exam")]
        public Guid ExamId { get; set; }
        public Exam Exam { get; set; }

    }
}