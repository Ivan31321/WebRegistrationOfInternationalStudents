using System.ComponentModel.DataAnnotations;

namespace MonitoringTheProgressOfForeignStudents.ViewModels.FacultyVM
{
    public class EditFacultyViewModel
    {
        [Required(ErrorMessage = "Необходимо указать название")]
        [MaxLength(80)]
        public string Name { get; set; }
    }
}
