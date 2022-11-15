using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.AspNetCore.Identity;
using GroupC.Uni.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace GroupC.Uni.Core.Entities
{
    [DisplayColumn("Name")]
    public class ApplicationUser : IdentityUser<Guid>, IAggregateRoot
    {
        public MyEnums.status Status { get; set; }
        public string CreationDate { get; set; }
        public string LastUpdateDate { get; set; }
        public string Name { get; set; }
        // public virtual ICollection<Role> Roles { get; set; }
        public virtual Admin Admin { get; set; }
        public virtual Student Student { get; set; }
        public virtual TestCenter TestCenter { get; set; }
        public MyEnums.UserType UserType { get; set; }
        public string ImageURL { get; set; }

    }
}
