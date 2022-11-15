using GroupC.Uni.Core.Entities;
using GroupC.Uni.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GroupC.Uni.Core.Services
{
    public class SubmissionService : Service<Submission>, ISubmissionService
    {
        private readonly ISubmissionRepository _submissionRepository;
        private readonly IAppLogger<IQuestionService> _logger;
        public SubmissionService(ISubmissionRepository submissionRepository, IAppLogger<IQuestionService> logger) : base(submissionRepository)
        {
            _submissionRepository = submissionRepository;
            _logger = logger;
        }

        public async Task CreateSubmissionAsync(Submission Submission)
        {
            await _submissionRepository.AddAsync(Submission);
        }

        public async Task<Submission> getByIdWithAll(Guid id)
        {
            return await _submissionRepository.getByIdWithAll(id);
        }

        public async Task<IReadOnlyList<Submission>> ListActiveSyncWithAll()
        {
            return await _submissionRepository.ListActiveSyncWithAll();
        }

        public async Task<List<Submission>> listSubmissionsByExamId(Guid id)
        {
            return await _submissionRepository.listSubmissionsByExamId(id);
        }

        public async Task<List<Submission>> listSubmissionsByStudentId(Guid id)
        {
            return await _submissionRepository.listSubmissionsByStudentId(id);
        }
    }
}