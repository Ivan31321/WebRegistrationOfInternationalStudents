using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonitoringTheProgressOfForeignStudents.Domain.Model
{
    [Table("order")]
    public class Order : BaseEntity
    {
        [Column("date")]
        public DateTime? Date { get; set; }
        [Column("order_content")]
        [MaxLength(150)]
        public string? OrderContent { get; set; }
        [Column("name")]
        [MaxLength(100)]
        public string? Name { get; set; }
        [Column("start_date")]
        public DateTime? StartDate { get; set; }
        [Column("order_type")]
        public OrderType? OrderType { get; set; }

        //link

        [Column("questionnaire_id")]
        public Guid? QuestionnaireId { get; set; }
        public Questionnaire? Questionnaire { get; set; }
    }

    public enum OrderType
    {
        Order,
        Reprimand
    }
}
