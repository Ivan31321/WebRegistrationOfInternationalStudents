using MonitoringTheProgressOfForeignStudents.Domain.Model;

namespace MonitoringTheProgressOfForeignStudents.ViewModels.StudentCardVM
{
    public class EditStudentCardViewModel
    {
        public Guid PersonalDetailsId { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? Surname { get; set; } = string.Empty;
        public string? Patronymic { get; set; } = string.Empty;
        public DateTime? Birthday { get; set; }
        public string? Gender { get; set; }
        public string? Country { get; set; }
        public string? Faculty { get; set; }
        public string? PlaceofBirth { get; set; } = string.Empty;

        public List<Home>? Homes { get; set; }

        public List<PassportInfo>? Passports { get; set; }
        public List<Register>? Registers { get; set; }
    }
}
