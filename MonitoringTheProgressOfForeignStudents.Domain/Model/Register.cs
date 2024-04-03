using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonitoringTheProgressOfForeignStudents.Domain.Model
{
    [Table("register")]
    public class Register : BaseEntity
    {
        [Column("info")]
        [MaxLength(50)]
        public string? Info { get; set; }
        [Column("valid_until")]
        public DateTime? ValidUntil { get; set; }

        [Column("student_card_id")]
        public Guid? StudentCardId { get; set; }
        public StudentCard? StudentCard { get; set; }
    }
}
