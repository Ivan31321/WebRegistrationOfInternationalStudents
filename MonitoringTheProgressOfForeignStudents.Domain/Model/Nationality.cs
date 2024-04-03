using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonitoringTheProgressOfForeignStudents.Domain.Model
{
    [Table("nationality")]
    public class Nationality : BaseEntity
    {
        [Column("name")]
        [MaxLength(50)]
        public string Name { get; set; }

        //link
        public ICollection<PersonalDetails> Persons { get; set; }
    }
}
