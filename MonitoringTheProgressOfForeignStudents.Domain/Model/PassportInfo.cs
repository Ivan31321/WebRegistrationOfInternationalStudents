using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonitoringTheProgressOfForeignStudents.Domain.Model
{
    [Table("passport_info")]
    public class PassportInfo : BaseEntity
    {
        [Column("number")]
        [MaxLength(40)]
        public string? Number { get; set; }
        [Column("date_of_issue")]
        public DateTime? DateOfIssue { get; set; }
        [Column("valid_until")]
        public DateTime? ValidUntil { get; set; }

        [Column("student_card_id")]
        public Guid? StudentCardId { get; set; }
        public StudentCard? StudentCard { get; set; }
    }
}
