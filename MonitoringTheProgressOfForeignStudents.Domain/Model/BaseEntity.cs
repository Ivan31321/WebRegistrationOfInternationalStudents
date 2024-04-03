using System.ComponentModel.DataAnnotations.Schema;

namespace MonitoringTheProgressOfForeignStudents.Domain.Model
{
    public abstract class BaseEntity
    {
        [Column("id")]
        public Guid Id { get; set; }
        [Column("created")]
        public DateTime? Created { get; set; }
    }
}
