namespace MonitoringTheProgressOfForeignStudents.ViewModels.GenderVM
{
    public class GetGenderListViewModel
    {
        public SearchGenderViewModel SearchGender { get; set; }
        public IEnumerable<GetGenderViewModel> GenderViewModels { get; set; }
    }
}
