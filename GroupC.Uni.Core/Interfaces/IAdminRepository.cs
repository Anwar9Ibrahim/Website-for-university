using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GroupC.Uni.Core.Entities;
namespace GroupC.Uni.Core.Interfaces
{
    public interface IAdminRepository : IAsyncRepository<Admin>
    {
        Task<Admin> GetAdminById(Guid id);
        Task<IReadOnlyList<Admin>> ListAllAdmins();
    }
}