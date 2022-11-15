using GroupC.Uni.Core.Entities;
using GroupC.Uni.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GroupC.Uni.Core.Services
{
    public class AdminService : Service<Admin>, IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IAppLogger<IAdminService> _logger;
        public AdminService(IAdminRepository adminRepositry, IAppLogger<IAdminService> logger) : base(adminRepositry)
        {
            _adminRepository = adminRepositry;
            _logger = logger;
        }
        public async Task<IReadOnlyList<Admin>> ListAllAdmins()
        {
            return await _adminRepository.ListAllAdmins();
        }
        public async Task<Admin> GetAdminById(Guid id)
        {
            return await _adminRepository.GetAdminById(id);
        }

       
    }
}
   