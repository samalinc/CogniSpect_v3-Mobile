using System.Threading.Tasks;
using System.Windows.Input;
using AutoMapper;
using CSMobile.Domain.Models.Tests;
using CSMobile.Domain.Services.Tests;
using CSMobile.Infrastructure.Common.Contexts.WebSocketSession;
using CSMobile.Infrastructure.Mvvm.Navigation;
using CSMobile.Infrastructure.Mvvm.ViewModelsCore;
using JetBrains.Annotations;

namespace CSMobile.Presentation.ViewModels.Tests
{
    [UsedImplicitly]
    public partial class TestViewModel : BasePageViewModel<Test>
    {
        private readonly INavigationService _navigationService;
        private readonly ITestsService _testsService;
        private readonly IMapper _mapper;
        private readonly IQuestionsService _questionsService;
        private readonly IWebSocketSessionService _webSocketSessionService;

        private int _questionNumber;
        
        public ICommand NextQuestionCommand { get; }
        public ICommand PreviousQuestionCommand { get; }
        public ICommand CompleteTestCommand { get; }

        public TestViewModel(
            INavigationService navigationService,
            ITestsService testsService,
            IMapper mapper,
            IQuestionsService questionsService,
            IWebSocketSessionService webSocketSessionService)
        {
            _navigationService = navigationService;
            _testsService = testsService;
            _mapper = mapper;
            _questionsService = questionsService;
            _webSocketSessionService = webSocketSessionService;

            NextQuestionCommand = Command(NextQuestion, this, false);
            PreviousQuestionCommand = Command(PreviousQuestion, this, false);
            CompleteTestCommand = Command(CompleteTest, this);
        }
        
        public override Task RetrieveArgument(Test argument)
        {
            _mapper.Map(argument, this);
            _questionNumber = 0;
            CurrentQuestion = Questions[_questionNumber];
            UpdateButtonsVisibility();
            return Task.CompletedTask;
        }

        private async Task NextQuestion()
        {
            await _questionsService.SendQuestionAnswer(_mapper.Map<ChoosableQuestion>(CurrentQuestion));
            CurrentQuestion = Questions[++_questionNumber];
            UpdateButtonsVisibility();
        }
        
        private Task PreviousQuestion()
        {
            CurrentQuestion = Questions[--_questionNumber];
            UpdateButtonsVisibility();
            
            return Task.CompletedTask;
        }

        private void UpdateButtonsVisibility()
        {
            // TODO: Logic will be added on demand
            IsPreviousButtonVisible = false;
            IsNextButtonVisible = _questionNumber != Questions.Count - 1;
            IsCompleteButtonVisible = !IsNextButtonVisible;
        }

        private async Task CompleteTest()
        {
            await _testsService.EndTest(_mapper.Map<Test>(this));
            await _navigationService.GoToRoot();
            await _webSocketSessionService.EndSession();
        }
    }
}