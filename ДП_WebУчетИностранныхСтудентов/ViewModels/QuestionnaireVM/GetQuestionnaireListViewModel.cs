namespace MonitoringTheProgressOfForeignStudents.ViewModels.QuestionnaireVM
{
    public class GetQuestionnaireListViewModel
    {
        public SearchQuestionnaireViewModel SearchQuestionnaire { get; set; }
        public List<GetQuestionnaireViewModel> QuestionnaireViewModels { get; set; }
    }
}
