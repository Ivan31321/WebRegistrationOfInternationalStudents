using System.ComponentModel.DataAnnotations;

namespace MonitoringTheProgressOfForeignStudents.ViewModels.Auth
{
    public class RegistrationViewModel
    {
        [Required(ErrorMessage = "Необходимо ввести имя")]
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Необходимо ввести фамилию")]
        [Display(Name = "Фамилия")]
        public string Surname { get; set; }

        [Display(Name = "Отвество")]
        public string? Patronymic { get; set; }

        [Required(ErrorMessage = "Необходимо ввести почту")]
        [EmailAddress(ErrorMessage = "Некорректный формат почты")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Необходимо ввести пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Необходимо ввести подтверждение пароля")]
        [Compare(nameof(Password), ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля")]
        public string ConfirmPassword { get; set; }
    }
}
