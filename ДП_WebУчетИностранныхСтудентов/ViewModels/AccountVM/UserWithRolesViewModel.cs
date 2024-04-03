using MonitoringTheProgressOfForeignStudents.Infrastructure.Identity;

namespace MonitoringTheProgressOfForeignStudents.ViewModels.AccountVM
{
    public class UserWithRolesViewModel
    {
        public User User { get; set; }
        public IList<string> Roles { get; set; }

        public override string ToString()
        {
            return $"{User.Surname} {User.Name} {User.Patronymic} {User.Email}";
        }
    }
}
