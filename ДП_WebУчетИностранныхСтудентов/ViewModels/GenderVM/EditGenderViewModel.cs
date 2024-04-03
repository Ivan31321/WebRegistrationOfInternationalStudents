using System.ComponentModel.DataAnnotations;

namespace MonitoringTheProgressOfForeignStudents.ViewModels.GenderVM
{
    public class EditGenderViewModel
    {
        [Required(ErrorMessage = "Необходимо указать пол")]
        [MaxLength(20)]
        public string Name { get; set; }
    }
}
