namespace MonitoringTheProgressOfForeignStudents.ViewModels.NationalityVM
{
    public class GetNationalityListViewModel
    {
        public SearchNationalityViewModel SearchNationality { get; set; }
        public IEnumerable<GetNationalityViewModel> NationalityViewModels { get; set; }
    }
}
