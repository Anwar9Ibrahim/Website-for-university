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
    public class AdminRepository : EfRepository<Admin>, IAdminRepository
    {

        public AdminRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Admin> GetAdminById(Guid id)
        {
            return await _dbContext.Admins.Include(s => s.ApplicationUser).Where(s => s.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<Admin>> ListAllAdmins()
        {
            return await _dbContext.Admins.Include(s => s.ApplicationUser).Where(s => s.userType == MyEnums.UserType.Admin).ToListAsync();

        }

    }
}

 