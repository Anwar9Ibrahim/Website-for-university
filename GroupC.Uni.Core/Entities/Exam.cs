using System;
using System.Collections.Generic;
using System.Text;
using GroupC.Uni.Core.Interfaces;

namespace GroupC.Uni.Core.Entities
{
	public class Exam : BaseEntity, IAggregateRoot
    {
        public Exam() : base()
        {
            ExamQuestions = new HashSet<ExamQuestion>();
            Submissions = new HashSet<Submission>();
        }
        public DateTime ExamDate { get; set; }
        public int DurationInMinutes { get; set; }
		public bool IsRandom { get; set; }
		public int QuestionsCount { get; set; }

        public Guid CourseId { get; set; }
        public virtual Course Course { get; set; }
        public Guid TestCenterId { get; set; }
        public virtual TestCenter TestCenter { get; set; }

        public virtual ICollection<ExamQuestion> ExamQuestions { get; set; }

        public virtual ICollection<Submission> Submissions { get; set; }

    }
}   
