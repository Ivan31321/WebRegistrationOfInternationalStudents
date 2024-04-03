using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonitoringTheProgressOfForeignStudents.Domain.Model
{
    [Table("education_type")]
    public class EducationType : BaseEntity
    {
        [Column("type")]
        [MaxLength(20)]
        public string Type { get; set; }

        //link
        public ICollection<Questionnaire> Questionnaires { get; set; }
    }
}
