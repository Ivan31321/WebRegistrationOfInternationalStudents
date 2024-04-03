namespace MonitoringTheProgressOfForeignStudents.ViewModels.EducationLanguageVM
{
    public class GetEducationLanguageListViewModel
    {
        public SearchEducationLanguageViewModel SearchEducationLanguage { get; set; }
        public IEnumerable<GetEducationLanguageViewModel> EducationLanguageViewModels { get; set; }
    }
}
