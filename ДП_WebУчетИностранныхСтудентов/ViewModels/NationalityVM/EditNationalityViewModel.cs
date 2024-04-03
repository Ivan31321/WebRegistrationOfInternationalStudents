using System.ComponentModel.DataAnnotations;

namespace MonitoringTheProgressOfForeignStudents.ViewModels.NationalityVM
{
    public class EditNationalityViewModel
    {
        [Required(ErrorMessage = "Необходимо указать национальность")]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
