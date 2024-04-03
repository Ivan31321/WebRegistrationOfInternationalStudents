namespace MonitoringTheProgressOfForeignStudents.ViewModels.SpecialtyVM
{
    public class GetSpecialtyListViewModel
    {
        public SearchSpecialtyViewModel SearchSpecialty { get; set; }
        public IEnumerable<GetSpecialtyViewModel> SpecialtyViewModels { get; set; }
    }
}
