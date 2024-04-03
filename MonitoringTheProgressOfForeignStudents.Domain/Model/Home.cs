using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonitoringTheProgressOfForeignStudents.Domain.Model
{
    [Table("home")]
    public class Home : BaseEntity
    {
        [Column("home_district")]
        [MaxLength(100)]
        public string? HomeDistrict { get; set; }
        [Column("home_address")]
        [MaxLength(150)]
        public string? HomeAddress { get; set; }
        [Column("home_flat")]
        [MaxLength(20)]
        public string? HomeFlat { get; set; }
        [Column("home_number")]
        [MaxLength()]
        public string? HomeNumber { get; set; }
        [Column("is_legacy")]
        public bool IsLegacy { get; set; }
        [Column("home_type")]
        public HomeType? HomeType { get; set; }

        [Column("questionnaire_id")]
        public Guid? QuestionnaireId { get; set; }
        public Questionnaire? Questionnaire { get; set; }
    }

    public enum HomeType
    {
        Apartment,
        Hostel
    }
}
