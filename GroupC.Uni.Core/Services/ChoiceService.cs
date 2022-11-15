using GroupC.Uni.Core.Entities;
using GroupC.Uni.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GroupC.Uni.Core.Services
{
    public class ChoiceService: Service<Choice>, IChoiceService
    {
        private readonly IChoiceRepository _choiceRepositry;
        private readonly IAppLogger<IQuestionService> _logger;
        public ChoiceService(IChoiceRepository choiceRepositry, IAppLogger<IQuestionService> logger) : base(choiceRepositry)
        {
            _choiceRepositry = choiceRepositry;
            _logger = logger;
        }
        public async Task CreateChoiceAsync(Choice choice)
        {
            await _choiceRepositry.AddAsync(choice);
        }
        public async Task<Choice> getByIdWithQuestion(Guid id)
        {
            return await _choiceRepositry.getByIdWithQuestion(id);
        }
        public async Task specialUpdateAsync(Choice Choice)
        {
            await _choiceRepositry.specialUpdateAsync(Choice);
        }
        public async Task<IReadOnlyList<Choice>> ListActiveSyncWithQuestion()
        {
            return await _choiceRepositry.ListActiveSyncWithQuestion();
        }

        public async Task<List<Choice>> listChoicesByQuestionId(Guid questionId)
        {
            return await _choiceRepositry.listChoicesByQuestionId(questionId);
        }
    }
}
