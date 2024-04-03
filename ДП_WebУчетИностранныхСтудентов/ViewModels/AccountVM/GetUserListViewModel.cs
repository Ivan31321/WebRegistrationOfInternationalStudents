namespace MonitoringTheProgressOfForeignStudents.ViewModels.AccountVM
{
    public class GetUserListViewModel
    {
        public SearchAccountViewModel SearchAccount { get; set; }
        public IEnumerable<UserWithRolesViewModel> UserWithRoles { get; set; }
    }
}
