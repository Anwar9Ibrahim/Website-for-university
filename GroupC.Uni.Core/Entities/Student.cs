using System;
using System.Collections.Generic;
using System.Text;
using GroupC.Uni.Core.Interfaces;

namespace GroupC.Uni.Core.Entities
{
    public class Student: BaseEntity, IAggregateRoot
    {
        public Student() : base()
        {
            Submissions = new HashSet<Submission>();
        }

        // public MyEnums.status Status { get; set; }
        // [ForeignKey("ApplicationUser")]
        //public string Id { get; set; }
        public int Year { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public Guid ApplicationUserId { get; set; }

        public virtual ICollection<Submission> Submissions { get; set; }
        //we need an object eather of user or of application user
        public MyEnums.UserType userType = MyEnums.UserType.Student;

    }
}
