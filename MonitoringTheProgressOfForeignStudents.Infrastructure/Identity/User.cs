using Microsoft.AspNetCore.Identity;

namespace MonitoringTheProgressOfForeignStudents.Infrastructure.Identity
{
    public class User : IdentityUser
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string? Patronymic { get; set; }

        public override string ToString()
        {
            return $"{Name} {Surname} {Patronymic} {Email}";
        }
    }
}
