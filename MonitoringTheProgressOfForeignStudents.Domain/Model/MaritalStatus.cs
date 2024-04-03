using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonitoringTheProgressOfForeignStudents.Domain.Model
{
    [Table("marital_status")]
    public class MaritalStatus : BaseEntity
    {
        [Column("name")]
        [MaxLength(50)]
        public string Name { get; set; }

        //link
        public ICollection<PersonalDetails> Persons { get; set; }
    }
}
