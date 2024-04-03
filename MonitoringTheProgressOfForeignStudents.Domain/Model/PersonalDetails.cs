using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonitoringTheProgressOfForeignStudents.Domain.Model
{
    [Table("personal_details")]
    public class PersonalDetails : BaseEntity
    {
        [Column("name")]
        [MaxLength(40)]
        public string Name { get; set; }
        [Column("surname")]
        [MaxLength(50)]
        public string? Surname { get; set; }
        [Column("patronymic")]
        [MaxLength(50)]
        public string? Patronymic { get; set; }
        [Column("birthday")]
        public DateTime Birthday { get; set; }
        [Column("place_of_birth")]
        [MaxLength(150)]
        public string PlaceofBirth { get; set; }
        [Column("permament_home_address")]
        [MaxLength(150)]
        public string PermamentHomeAddress { get; set; }
        [Column("phone_number")]
        [MaxLength(20)]
        [Phone]
        public string? PhoneNumber { get; set; }
        [Column("phone_number_in_belarus")]
        [MaxLength(20)]
        [Phone]
        public string? PhoneNumberInBelarus { get; set; }
        [Column("email")]
        [MaxLength(50)]
        [EmailAddress]
        public string? Email { get; set; }

        //link
        [Column("nationality_id")]
        public Guid? NationalityId { get; set; }
        public Nationality? Nationality { get; set; }
        [Column("maritalStatus_id")]
        public Guid? MaritalStatusId { get; set; }
        public MaritalStatus? MaritalStatus { get; set; }
        [Column("gender_id")]
        public Guid? GenderId { get; set; }
        public Gender? Gender { get; set; }
        [Column("country_id")]
        public Guid? CountryId { get; set; }
        public Country? Country { get; set; }

        //history and actual
        public List<Questionnaire> Questionnaires { get; set; }
        public Questionnaire QuestionnaireLatest
        {
            get
            {
                return Questionnaires.OrderByDescending(x => x.Created).First();
            }
        }

        public List<StudentCard> StudentCards { get; set; }
        public StudentCard StudentCardLatest
        {
            get
            {
                return StudentCards.OrderByDescending(x => x.Created).First();
            }
        }
    }
}
