using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonitoringTheProgressOfForeignStudents.Domain.Model
{
    [Table("questionnaire")]
    public class Questionnaire : BaseEntity
    {
        [Column("fathers_name")]
        [MaxLength(150)]
        public string? FathersName { get; set; }
        [Column("fathers_profession")]
        [MaxLength(50)]
        public string? FathersProfession { get; set; }
        [Column("fathers_phone")]
        [MaxLength(20)]
        public string? FathersPhone { get; set; }
        [Column("mothers_name")]
        [MaxLength(150)]
        public string? MothersName { get; set; }
        [Column("mothers_profession")]
        [MaxLength(50)]
        public string? MothersProfession { get; set; }
        [Column("mothers_phone")]
        [MaxLength(20)]
        public string? MothersPhone { get; set; }
        [Column("name_of_school")]
        [MaxLength(50)]
        public string? NameOfSchool { get; set; }
        [Column("address_of_school")]
        [MaxLength(150)]
        public string? AddressOfSchool { get; set; }
        [Column("date_of_entrance")]
        public DateTime? DateOfEntrance { get; set; }
        [Column("date_of_completion")]
        public DateTime? DateOfCompletion { get; set; }
        [Column("knowledge_of_languages")]
        [MaxLength(200)]
        public string? KnowledgeOfLanguages { get; set; }
        [Column("relative_to_be_informed")]
        [MaxLength(200)]
        public string? RelativeToBeInformedInEmergency { get; set; }
        [Column("how_did_find_bntu")]
        [MaxLength(200)]
        public string? HowDidYouFindOutAboutBNTU { get; set; }

        //link
        [Column("personal_details_id")]
        public Guid PersonalDetailsId { get; set; }
        public PersonalDetails PersonalDetails { get; set; }
        [Column("specialty_id")]
        public Guid? SpecialtyId { get; set; }
        public Specialty? Specialty { get; set; }
        [Column("education_language_id")]
        public Guid? EducationLanguageId { get; set; }
        public EducationLanguage? EducationLanguage { get; set; }
        [Column("education_type_id")]
        public Guid? EducationTypeId { get; set; }
        public EducationType? EducationType { get; set; }

        public ICollection<Order> Orders { get; set; }

        //additional
        public ICollection<Home> Homes { get; set; }
    }
}