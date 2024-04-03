namespace MonitoringTheProgressOfForeignStudents.ViewModels.EducationTypeVM
{
    public class GetEducationTypeListViewModel
    {
        public SearchEducationTypeViewModel SearchEducationType { get; set; }
        public IEnumerable<GetEducationTypeViewModel> EducationTypeViewModels { get; set; }
    }
}
