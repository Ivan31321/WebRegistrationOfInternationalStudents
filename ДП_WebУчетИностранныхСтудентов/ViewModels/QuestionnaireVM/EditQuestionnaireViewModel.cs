using MonitoringTheProgressOfForeignStudents.Domain.Model;
using System.ComponentModel.DataAnnotations;

namespace MonitoringTheProgressOfForeignStudents.ViewModels.QuestionnaireVM
{
    public class EditQuestionnaireViewModel
    {
        public Guid? PersonalDetailsId { get; set; }
        [MaxLength(40)]
        public string Name { get; set; } = string.Empty;
        [MaxLength(50)]
        public string? Surname { get; set; } = string.Empty;
        [MaxLength(50)]
        public string? Patronymic { get; set; } = string.Empty;
        [Required(ErrorMessage = "Необходимо указать дату рождения")]
        public DateTime Birthday { get; set; }
        [Required(ErrorMessage = "Необходимо указать место рождения")]
        [MaxLength(150)]
        public string PlaceofBirth { get; set; } = string.Empty;
        [Required(ErrorMessage = "Необходимо указать постоянный адрес проживания")]
        [MaxLength(150)]
        public string PermamentHomeAddress { get; set; } = string.Empty;
        [MaxLength(20)]
        public string? PhoneNumber { get; set; } = string.Empty;
        [MaxLength(20)]
        public string? PhoneNumberInBelarus { get; set; } = string.Empty;
        [MaxLength(50)]
        public string? Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Необходимо указать пол")]
        public Guid? GenderId { get; set; }
        [Required(ErrorMessage = "Необходимо указать страну")]
        public Guid? CountryId { get; set; }
        [Required(ErrorMessage = "Необходимо указать национальность")]
        public Guid? NationalityId { get; set; }
        [Required(ErrorMessage = "Необходимо указать семейное положение")]
        public Guid? MaritalStatusId { get; set; }
        //
        [MaxLength(150)]
        public string? FathersName { get; set; } = string.Empty;
        [MaxLength(50)]
        public string? FathersProfession { get; set; } = string.Empty;
        [MaxLength(20)]
        public string? FathersPhone { get; set; } = string.Empty;
        [MaxLength(150)]
        public string? MothersName { get; set; } = string.Empty;
        [MaxLength(50)]
        public string? MothersProfession { get; set; } = string.Empty;
        [MaxLength(20)]
        public string? MothersPhone { get; set; } = string.Empty;
        [MaxLength(50)]
        public string? NameOfSchool { get; set; } = string.Empty;
        [MaxLength(150)]
        public string? AddressOfSchool { get; set; } = string.Empty;
        public DateTime? DateOfEntrance { get; set; }
        public DateTime? DateOfCompletion { get; set; }
        [MaxLength(200)]
        public string? KnowledgeOfLanguages { get; set; } = string.Empty;
        [MaxLength(200)]
        public string? RelativeToBeInformedInEmergency { get; set; } = string.Empty;
        [MaxLength(200)]
        public string? HowDidYouFindOutAboutBNTU { get; set; } = string.Empty;

        //
        [Required(ErrorMessage = "Необходимо указать специальность")]
        public Guid? SpecialtyId { get; set; }
        [Required(ErrorMessage = "Необходимо указать язык образования")]
        public Guid? EducationLanguageId { get; set; }
        [Required(ErrorMessage = "Необходимо указать тип образования")]
        public Guid? EducationTypeId { get; set; }

        public List<Order>? Orders { get; set; }
        public List<Order>? Reprimands { get; set; }

        //
        public List<Home>? Homes { get; set; }
    }
}
