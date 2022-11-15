using System;
using System.Collections.Generic;
using System.Text;
using GroupC.Uni.Core.Interfaces;
namespace GroupC.Uni.Core.Entities
{
	public class Admin : BaseEntity, IAggregateRoot
    {
       public virtual ApplicationUser ApplicationUser { get; set; }
        public Guid ApplicationUserId { get; set; }
        public MyEnums.UserType userType = MyEnums.UserType.Admin;
    }
}
