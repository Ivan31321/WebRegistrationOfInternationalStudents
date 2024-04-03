using System.ComponentModel.DataAnnotations.Schema;

namespace MonitoringTheProgressOfForeignStudents.Domain.Model
{
    [Table("student_card")]
    public class StudentCard : BaseEntity
    {
        [Column("personal_details_id")]
        public Guid PersonalDetailsId { get; set; }
        public PersonalDetails PersonalDetails { get; set; }
        public ICollection<PassportInfo> Passports { get; set; }
        public ICollection<Register> Registers { get; set; }
    }
}
