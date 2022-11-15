using GroupC.Uni.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroupC.Uni.Student.Model
{
    public class StudentModel
    {
        public Guid guid { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }
        //public string RefreshToken { get; set; }
    }
}
