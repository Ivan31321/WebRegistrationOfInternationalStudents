using System.ComponentModel.DataAnnotations;

namespace MonitoringTheProgressOfForeignStudents.ViewModels.PersonalDetailsVM
{
    public class EditPersonalDetailsViewModel
    {
        [Required(ErrorMessage = "Необходимо указать имя")]
        [MaxLength(40)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string? Surname { get; set; }
        [MaxLength(50)]
        public string? Patronymic { get; set; }

        [Required(ErrorMessage = "Необходимо указать дату рождения")]
        public DateTime Birthday { get; set; }

        [Required(ErrorMessage = "Необходимо указать место рождения")]
        [MaxLength(150)]
        public string PlaceofBirth { get; set; }

        [Required(ErrorMessage = "Необходимо указать постоянный адрес проживания")]
        [MaxLength(150)]
        public string PermamentHomeAddress { get; set; }
        [MaxLength(20)]
        public string? PhoneNumber { get; set; }
        [MaxLength(20)]
        public string? PhoneNumberInBelarus { get; set; }
        [MaxLength(50)]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Необходимо указать национальность")]
        public Guid? NationalityId { get; set; }

        [Required(ErrorMessage = "Необходимо указать семейное положение")]
        public Guid? MaritalStatusId { get; set; }

        [Required(ErrorMessage = "Необходимо указать пол")]
        public Guid? GenderId { get; set; }

        [Required(ErrorMessage = "Необходимо указать страну")]
        public Guid? CountryId { get; set; }
    }
}
