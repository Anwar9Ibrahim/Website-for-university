using GroupC.Uni.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GroupC.Uni.Core.Interfaces
{
    public interface ITestCenterService : IService<TestCenter>
    {
        Task<TestCenter> GetTestCenterById(Guid id);
        Task<IReadOnlyList<TestCenter>> ListAllTestCenters();
        List<TestCenter> GetAllAsListWithAppUser();
    }
}
