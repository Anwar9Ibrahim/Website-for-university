using GroupC.Uni.Core.Entities;
using GroupC.Uni.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace GroupC.Uni.Infrastructure.Data
{
    public class StudentRepository : EfRepository<Student>, IStudentRepository
    {
       
        public StudentRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Student> GetStudentById(Guid id)
        {
            return await _dbContext.Students.Include(s => s.ApplicationUser).Where(s => s.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<Student>> ListAllStudents()
        {
            return await _dbContext.Students.Include(s => s.ApplicationUser).Where(s => s.userType == MyEnums.UserType.Student).ToListAsync();

        }

    }

}