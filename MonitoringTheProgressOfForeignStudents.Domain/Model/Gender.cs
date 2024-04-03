using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonitoringTheProgressOfForeignStudents.Domain.Model
{
    [Table("gender")]
    public class Gender : BaseEntity
    {
        [Column("name")]
        [MaxLength(20)]
        public string Name { get; set; }

        //link
        public ICollection<PersonalDetails> Persons { get; set; }
    }
}
