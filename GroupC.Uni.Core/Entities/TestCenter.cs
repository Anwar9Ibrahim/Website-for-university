using GroupC.Uni.Core.Interfaces;
using System;
using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GroupC.Uni.Core.Entities
{
    public class TestCenter : BaseEntity, IAggregateRoot
    {
        public TestCenter() : base()
        {

            Exams = new HashSet<Exam>();
        }
        public virtual ICollection<Exam> Exams { get; set; }
        // [ForeignKey("ApplicationUser")]
        // public string Id { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public string Name { get; set; }

        public Guid ApplicationUserId { get; set; }
        public MyEnums.UserType userType = MyEnums.UserType.TestCenter;
    }
}
