using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GroupC.Uni.Core.Entities;

namespace GroupC.Uni.Core.Interfaces
{
    public interface IStudentRepository: IAsyncRepository<Student>
    {
        Task<Student> GetStudentById(Guid id);
        Task<IReadOnlyList<Student>> ListAllStudents();
    }
}
