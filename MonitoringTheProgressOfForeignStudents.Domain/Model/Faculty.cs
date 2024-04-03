using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonitoringTheProgressOfForeignStudents.Domain.Model
{
    [Table("faculty")]
    public class Faculty : BaseEntity
    {
        [Column("name")]
        [MaxLength(80)]
        public string Name { get; set; }

        //link
        public ICollection<Specialty> Specialties { get; set; }
    }
}
