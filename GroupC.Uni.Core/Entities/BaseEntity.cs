using System;
using System.Collections.Generic;
using System.Text;
namespace GroupC.Uni.Core.Entities
{
    public class BaseEntity
        {
            public Guid Id { get; set; }
            public DateTime LastUpdateDate { get; set; }
            public DateTime CreationDate { get; set; }
            public MyEnums.status Status { get; set; }
        }
    }
