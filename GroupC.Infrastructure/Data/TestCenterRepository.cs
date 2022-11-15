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
    public class TestCenterRepository : EfRepository<TestCenter>, ITestCenterRepository
    {

        public TestCenterRepository(AppDbContext dbContext) : base(dbContext)
        {
        }


        public virtual async Task<TestCenter> GetTestCenterById(Guid id)
        {
            return await _dbContext.TestCenters.Include(t => t.ApplicationUser).Where(t => t.Id == id).FirstOrDefaultAsync();

           // return await _dbContext.TestCenters.Include(e => e.ApplicationUser).Where(e => e.ApplicationUserId == id).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<TestCenter>> ListAllTestCenters()
        {
            return await _dbContext.TestCenters.Include(s => s.ApplicationUser).Where(s => s.userType == MyEnums.UserType.TestCenter).ToListAsync();

        }
        public List<TestCenter> GetAllAsListWithAppUser()
        {
            return _dbContext.TestCenters.Include(s => s.ApplicationUser).Where(s => s.Status == MyEnums.status.Active).ToList();
        }
    }
}