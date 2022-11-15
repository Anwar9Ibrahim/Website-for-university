using GroupC.Uni.Core.Entities;
using GroupC.Uni.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GroupC.Uni.Core.Services
{
    public class StudentService: Service<Student>, IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IAppLogger<IStudentService> _logger;
        public StudentService(IStudentRepository studentRepositry, IAppLogger<IStudentService> logger) : base(studentRepositry)
        {
            _studentRepository = studentRepositry;
            _logger = logger;
        }
        public async Task<IReadOnlyList<Student>> ListAllStudents()
        {
            return await _studentRepository.ListAllStudents();
        }
        public async Task<Student> GetStudentById(Guid id)
        {
            return await _studentRepository.GetStudentById(id);
        }
    }
}
