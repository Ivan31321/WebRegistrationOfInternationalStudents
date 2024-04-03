using System.ComponentModel.DataAnnotations;

namespace MonitoringTheProgressOfForeignStudents.ViewModels.Auth
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Необходимо ввести почту")]
        [EmailAddress(ErrorMessage = "Некорректный формат почты")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Необходимо ввести пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Display(Name = "Запомнить?")]
        public bool RememberMe { get; set; }
    }
}
