namespace MonitoringTheProgressOfForeignStudents.ViewModels.MaritalStatusVM
{
    public class GetMaritalStatusListViewModel
    {
        public SearchMaritalStatusViewModel SearchMaritalStatus { get; set; }
        public IEnumerable<GetMaritalStatusViewModel> MaritalStatusViewModels { get; set; }
    }
}
