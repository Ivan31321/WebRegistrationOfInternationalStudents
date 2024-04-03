namespace MonitoringTheProgressOfForeignStudents.ViewModels.FacultyVM
{
    public class GetFacultyListViewModel
    {
        public SearchFacultyViewModel SearchFaculty { get; set; }
        public IEnumerable<GetFacultyViewModel> FacultyViewModels { get; set; }
    }
}
