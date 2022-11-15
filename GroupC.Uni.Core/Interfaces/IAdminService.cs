using GroupC.Uni.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
namespace GroupC.Uni.Core.Interfaces
{
    public interface IAdminService: IService<Admin>
    {
        Task<Admin> GetAdminById(Guid id);
        Task<IReadOnlyList<Admin>> ListAllAdmins();
    }
}
