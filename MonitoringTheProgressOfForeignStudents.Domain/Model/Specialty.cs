using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonitoringTheProgressOfForeignStudents.Domain.Model
{
    [Table("specialty")]
    public class Specialty : BaseEntity
    {
        [Column("code")]
        [MaxLength(20)]
        public string Code { get; set; }
        [Column("name")]
        [MaxLength(100)]
        public string Name { get; set; }

        //link
        [Column("faculty_id")]
        public Guid? FacultyId { get; set; }
        public Faculty? Faculty { get; set; }

        public ICollection<Questionnaire> Questionnaires { get; set; }
    }
}
