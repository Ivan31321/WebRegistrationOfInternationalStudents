using System.ComponentModel.DataAnnotations;

namespace MonitoringTheProgressOfForeignStudents.ViewModels.SpecialtyVM
{
    public class EditSpecialtyViewModel
    {
        [Required(ErrorMessage = "Необходимо указать название")]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Необходимо указать код")]
        [MaxLength(20)]
        public string Code { get; set; }
        [Required(ErrorMessage = "Необходимо указать факультет")]
        public Guid? FacultyId { get; set; }
    }
}
