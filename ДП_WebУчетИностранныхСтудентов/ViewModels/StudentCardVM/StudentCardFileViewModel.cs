using DocumentFormat.OpenXml.Spreadsheet;
using MonitoringTheProgressOfForeignStudents.Domain.Model;

namespace MonitoringTheProgressOfForeignStudents.ViewModels.StudentCardVM
{
    public class StudentCardFileViewModel
    {
        public string? Name { get; set; } = string.Empty;
        public string? Surname { get; set; } = string.Empty;
        public string? Patronymic { get; set; } = string.Empty;
        public DateTime? Birthday { get; set; }
        public string? Gender { get; set; }
        public string? Country { get; set; }
        public string? Faculty { get; set; }
        public string? PlaceofBirth { get; set; } = string.Empty;

        public List<HomeViewModel>? Homes { get; set; }
        public List<PassportInfoViewModel>? Passports { get; set; }
        public List<RegisterViewModel>? Registers { get; set; }

        public StudentCardFileViewModel(EditStudentCardViewModel cardViewModel)
        {
            Name = cardViewModel.Name;
            Surname = cardViewModel.Surname;
            Patronymic = cardViewModel.Patronymic;
            Birthday = cardViewModel.Birthday;
            Gender = cardViewModel.Gender;
            Country = cardViewModel.Country;
            Faculty = cardViewModel.Faculty;
            PlaceofBirth = cardViewModel.PlaceofBirth;
        }

        private string _emptyField = "Не указано";

        public Dictionary<string, string> ToFileDict()
        {
            var dict = new Dictionary<string, string>()
            {
                {"!name!", Name ?? _emptyField },
                {"!surname!", Surname ?? _emptyField },
                {"!patronymic!", Patronymic ?? _emptyField },
                {"!birthday!", Birthday?.ToString("dd/MM/yyyy") ?? _emptyField },
                {"!placeOfBirth!", PlaceofBirth ?? _emptyField },
                {"!gender!", Gender ?? _emptyField },
                {"!country!", Country ?? _emptyField },
                {"!faculty!", Faculty ?? _emptyField },
            };

            return dict;
        }
    }

    public class HomeViewModel
    {
        public class Home : BaseEntity
        {
            public string HomeDistrict { get; set; }
            public string HomeAddress { get; set; }
            public string HomeFlat { get; set; }
            public string HomeNumber { get; set; }
            public bool IsLegacy { get; set; }
            public string HomeType { get; set; }
        }
    }

    public class PassportInfoViewModel
    {
        public string Number { get; set; }
        public DateTime DateOfIssue { get; set; }
        public DateTime ValidUntil { get; set; }
    }

    public class RegisterViewModel
    {
        public string Info { get; set; }
        public DateTime ValidUntil { get; set; }
    }
}
