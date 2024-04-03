namespace MonitoringTheProgressOfForeignStudents.ViewModels.PersonalDetailsVM
{
    public class GetPersonalDetailsViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public string Email { get; set; }
        public string Nationality { get; set; }
        public string Gender { get; set; }
        public string MaritalStatus { get; set; }
        public string Country { get; set; }

        public override string ToString()
        {
            return $"{Name} {Surname} {Patronymic} {Email} {Nationality} {Gender} {MaritalStatus} {Country}";
        }
    }
}
