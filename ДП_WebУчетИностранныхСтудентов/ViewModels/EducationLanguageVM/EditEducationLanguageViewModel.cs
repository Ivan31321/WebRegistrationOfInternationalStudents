using System.ComponentModel.DataAnnotations;

namespace MonitoringTheProgressOfForeignStudents.ViewModels.EducationLanguageVM
{
    public class EditEducationLanguageViewModel
    {
        [Required(ErrorMessage = "Необходимо указать язык")]
        [MaxLength(20)]
        public string Language { get; set; }
    }
}
