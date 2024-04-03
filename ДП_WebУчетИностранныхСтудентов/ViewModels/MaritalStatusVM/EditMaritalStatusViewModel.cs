using System.ComponentModel.DataAnnotations;

namespace MonitoringTheProgressOfForeignStudents.ViewModels.MaritalStatusVM
{
    public class EditMaritalStatusViewModel
    {
        [Required(ErrorMessage = "Необходимо указать семейное положение")]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
