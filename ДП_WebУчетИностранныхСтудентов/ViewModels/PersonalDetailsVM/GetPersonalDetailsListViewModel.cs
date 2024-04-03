namespace MonitoringTheProgressOfForeignStudents.ViewModels.PersonalDetailsVM
{
    public class GetPersonalDetailsListViewModel
    {
        public SearchPersonalDetailsViewModel SearchPersonalDetails { get; set; }
        public IEnumerable<GetPersonalDetailsViewModel> PersonalDetailsViewModels { get; set; }
    }
}
