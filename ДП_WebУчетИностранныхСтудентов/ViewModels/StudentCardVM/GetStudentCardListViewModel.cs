namespace MonitoringTheProgressOfForeignStudents.ViewModels.StudentCardVM
{
    public class GetStudentCardListViewModel
    {
        public SearchStudentCardViewModel SearchStudentCard { get; set; }
        public IEnumerable<GetStudentCardViewModel> StudentCardViewModels { get; set; }
    }
}
