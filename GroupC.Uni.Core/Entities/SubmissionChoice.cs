using GroupC.Uni.Core.Interfaces;
using System;
using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GroupC.Uni.Core.Entities
{
    public class SubmissionChoice : BaseEntity, IAggregateRoot
    {
        public Guid ChoiceId { get; set; }
        public Choice Choice { get; set; }
        //[ForeignKey("Submission")]
        public Guid SubmissionId { get; set; }
        public Submission Submission { get; set; }


    }
}
