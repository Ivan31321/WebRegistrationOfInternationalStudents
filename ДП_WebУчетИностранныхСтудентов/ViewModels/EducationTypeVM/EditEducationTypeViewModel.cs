using System.ComponentModel.DataAnnotations;

namespace MonitoringTheProgressOfForeignStudents.ViewModels.EducationTypeVM
{
    public class EditEducationTypeViewModel
    {
        [Required(ErrorMessage = "Необходимо указать тип")]
        [MaxLength(20)]
        public string Type { get; set; }
    }
}
