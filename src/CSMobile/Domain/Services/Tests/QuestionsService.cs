using System.Threading.Tasks;
using CSMobile.Domain.Models.Tests;
using JetBrains.Annotations;

namespace CSMobile.Domain.Services.Tests
{
    [UsedImplicitly]
    internal class QuestionsService : IQuestionsService
    {
        public Task SendQuestionAnswer(Question question)
        {
            //TODO: will be implemented on demand
            return Task.CompletedTask;
        }
    }
}