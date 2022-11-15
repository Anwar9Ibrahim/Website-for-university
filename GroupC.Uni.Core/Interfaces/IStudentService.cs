using GroupC.Uni.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GroupC.Uni.Core.Interfaces
{
    public interface IStudentService: IService<Student>
    {
        Task<Student> GetStudentById(Guid id);
        Task<IReadOnlyList<Student>> ListAllStudents();
    }
}
