namespace MonitoringTheProgressOfForeignStudents.ViewModels.QuestionnaireVM
{
    public class QuestionnaireFileViewModel
    {
        public string Name { get; set; } = string.Empty;
        public string? Surname { get; set; } = string.Empty;
        public string? Patronymic { get; set; } = string.Empty;
        public DateTime Birthday { get; set; }
        public string PlaceofBirth { get; set; } = string.Empty;
        public string PermamentHomeAddress { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; } = string.Empty;
        public string? PhoneNumberInBelarus { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;

        public string? FathersName { get; set; } = string.Empty;
        public string? FathersProfession { get; set; } = string.Empty;
        public string? FathersPhone { get; set; } = string.Empty;
        public string? MothersName { get; set; } = string.Empty;
        public string? MothersProfession { get; set; } = string.Empty;
        public string? MothersPhone { get; set; } = string.Empty;
        public string? NameOfSchool { get; set; } = string.Empty;
        public string? AddressOfSchool { get; set; } = string.Empty;
        public DateTime? DateOfEntrance { get; set; }
        public DateTime? DateOfCompletion { get; set; }
        public string? KnowledgeOfLanguages { get; set; } = string.Empty;
        public string? RelativeToBeInformedInEmergency { get; set; } = string.Empty;
        public string? HowDidYouFindOutAboutBNTU { get; set; } = string.Empty;

        public string? Specialty { get; set; }
        public string? Gender { get; set; }
        public string? Country { get; set; }
        public string? MaritalStatus { get; set; }
        public string? Nationality { get; set; }

        private string _emptyField = "Не указано";

        public QuestionnaireFileViewModel(EditQuestionnaireViewModel editQuestionnaire)
        {
            Name = editQuestionnaire.Name;
            Surname = editQuestionnaire.Surname;
            Patronymic = editQuestionnaire.Patronymic;
            Birthday = editQuestionnaire.Birthday;
            PlaceofBirth = editQuestionnaire.PlaceofBirth;
            PermamentHomeAddress = editQuestionnaire.PermamentHomeAddress;
            PhoneNumber = editQuestionnaire.PhoneNumber;
            PhoneNumberInBelarus = editQuestionnaire.PhoneNumberInBelarus;
            Email = editQuestionnaire.Email;
            FathersName = editQuestionnaire.FathersName;
            FathersPhone = editQuestionnaire.FathersPhone;
            FathersProfession = editQuestionnaire.FathersProfession;
            MothersName = editQuestionnaire.MothersName;
            MothersPhone = editQuestionnaire.MothersPhone;
            MothersProfession = editQuestionnaire.MothersProfession;
            NameOfSchool = editQuestionnaire.NameOfSchool;
            AddressOfSchool = editQuestionnaire.AddressOfSchool;
            DateOfEntrance = editQuestionnaire.DateOfEntrance;
            DateOfCompletion = editQuestionnaire.DateOfCompletion;
            KnowledgeOfLanguages = editQuestionnaire.KnowledgeOfLanguages;
            RelativeToBeInformedInEmergency = editQuestionnaire.RelativeToBeInformedInEmergency;
            HowDidYouFindOutAboutBNTU = editQuestionnaire.HowDidYouFindOutAboutBNTU;
        }

        public Dictionary<string, string> ToFileDict()
        {
            var dict = new Dictionary<string, string>()
            {
                {"!name!", Name },
                {"!surname!", Surname ?? _emptyField },
                {"!patronymic!", Patronymic ?? _emptyField },
                {"!birthday!", Birthday.ToString("dd/MM/yyyy") },
                {"!placeOfBirth!", PlaceofBirth },
                {"!permamentHome!", PermamentHomeAddress },
                {"!phone!", PhoneNumber ?? _emptyField },
                {"!phoneInBelarus!", PhoneNumberInBelarus ?? _emptyField },
                {"!email!", Email ?? _emptyField },
                {"!fathersName!", FathersName ?? _emptyField },
                {"!fathersPhone!", FathersPhone ?? _emptyField },
                {"!fathersProf!", FathersProfession ?? _emptyField},
                {"!mothersName!", MothersName ?? _emptyField },
                {"!mothersPhone!", MothersPhone ?? _emptyField },
                {"!mothersProf!", MothersProfession ?? _emptyField },
                {"!nameOfSchool!", NameOfSchool ?? _emptyField },
                {"!addressOfSchool!", AddressOfSchool ?? _emptyField },
                {"!knownLang!", KnowledgeOfLanguages ?? _emptyField },
                {"!dateOfEntrance!", DateOfEntrance?.ToString("dd/MM/yyyy") ?? _emptyField },
                {"!dateOfCompletion!", DateOfCompletion?.ToString("dd/MM/yyyy") ?? _emptyField },
                {"!relativeToBeInform!", RelativeToBeInformedInEmergency ?? _emptyField },
                {"!howDidFind!", HowDidYouFindOutAboutBNTU ?? _emptyField },
                {"!gender!", Gender ?? _emptyField },
                {"!country!", Country ?? _emptyField },
                {"!maritalStatus!", MaritalStatus ?? _emptyField },
                {"!nationality!", Nationality ?? _emptyField },
                {"!specialty!", Specialty ?? _emptyField },
            };

            return dict;
        }
    }
}
