using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonitoringTheProgressOfForeignStudents.Domain.Model
{
    [Table("education_language")]
    public class EducationLanguage : BaseEntity
    {
        [Column("language")]
        [MaxLength(20)]
        public string Language { get; set; }

        //link
        public ICollection<Questionnaire> Questionnaires { get; set; }
    }
}
