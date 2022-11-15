using GroupC.Uni.Core.Entities;
using GroupC.Uni.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GroupC.Uni.Core.Services
{
    public class TestCenterService : Service<TestCenter>, ITestCenterService
    {
        private readonly ITestCenterRepository _testCenterRepository;
        private readonly IAppLogger<ITestCenterService> _logger;
        public TestCenterService(ITestCenterRepository testCenterRepositry, IAppLogger<ITestCenterService> logger) : base(testCenterRepositry)
        {
            _testCenterRepository = testCenterRepositry;
            _logger = logger;
        }

        public async Task<TestCenter> GetTestCenterById(Guid id)
        {
            return await _testCenterRepository.GetTestCenterById(id);
        }

        public async Task<IReadOnlyList<TestCenter>> ListAllTestCenters()
        {
            return await _testCenterRepository.ListAllTestCenters();
        }
        public List<TestCenter> GetAllAsListWithAppUser()
        {
            return _testCenterRepository.GetAllAsListWithAppUser();
        }
    }
}
