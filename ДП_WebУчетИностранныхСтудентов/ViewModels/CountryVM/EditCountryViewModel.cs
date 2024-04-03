using System.ComponentModel.DataAnnotations;

namespace MonitoringTheProgressOfForeignStudents.ViewModels.CountryVM
{
    public class EditCountryViewModel
    {
        [Required(ErrorMessage = "Необходимо указать название")]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
