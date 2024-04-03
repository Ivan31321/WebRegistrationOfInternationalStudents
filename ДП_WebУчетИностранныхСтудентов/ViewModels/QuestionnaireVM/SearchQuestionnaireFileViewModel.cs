namespace MonitoringTheProgressOfForeignStudents.ViewModels.QuestionnaireVM
{
    public class SearchQuestionnaireFileViewModel
    {
        public string? Gender { get; set; }
        public string? Specialty { get; set; }
        public string? Country { get; set; }
        public string? Nationality { get; set; }
        public string? MaritalStatus { get; set; }
        public string? EducationLanguage { get; set; }
        public string? EducationType { get; set; }
        public string? SearchString { get; set; }
        public IEnumerable<GetQuestionnaireViewModel> Questionnaires { get; set; }

        private string _emptyField = "Не определено";

        public SearchQuestionnaireFileViewModel(GetQuestionnaireListViewModel viewModel)
        {
            SearchString = viewModel.SearchQuestionnaire.SearchString;
            Questionnaires = viewModel.QuestionnaireViewModels;
        }

        public Dictionary<string, string> ToFileDict()
        {
            if(Questionnaires == null) Questionnaires = new List<GetQuestionnaireViewModel>();

            var dict = new Dictionary<string, string>()
            {
                {"!gender!", Gender ?? _emptyField },
                {"!country!", Country ?? _emptyField },
                {"!maritalStatus!", MaritalStatus ?? _emptyField },
                {"!nationality!", Nationality ?? _emptyField },
                {"!specialty!", Specialty ?? _emptyField },
                {"!eduLang!", EducationLanguage ?? _emptyField },
                {"!eduType!", EducationType ?? _emptyField },
                {"!search!", SearchString ?? _emptyField },
                {"!body!", string.Join($"</w:t><w:br/><w:t>", Questionnaires.Select(x=>x.ToString())) ?? _emptyField },
            };

            return dict;
        }
    }
}
