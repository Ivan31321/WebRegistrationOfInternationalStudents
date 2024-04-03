namespace MonitoringTheProgressOfForeignStudents.ViewModels.StudentCardVM
{
    public class ExcelStudentCardViewModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public string Nationality { get; set; }
        public string Gender { get; set; }
        public string MaritalStatus { get; set; }
        public bool PassportValid { get; set; }
        public bool RegisterValid { get; set; }
    }
}
