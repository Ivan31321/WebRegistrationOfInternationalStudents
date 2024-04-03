﻿namespace MonitoringTheProgressOfForeignStudents.ViewModels.QuestionnaireVM
{
    public class GetQuestionnaireViewModel
    {
        public Guid Id { get; set; }
        public Guid PersonalId { get; set; }
        public DateTime? Created { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Patronymic { get; set; }
        public string? Email { get; set; }
        public string? Nationality { get; set; }
        public string? Gender { get; set; }
        public string? MaritalStatus { get; set; }
        public string? Country { get; set; }
        public string? Specialty { get; set; }
        public string? SpecialtyCode { get; set; }
        public string? Faculty { get; set; }

        public override string ToString()
        {
            return $"{Name} {Surname} {Patronymic} | \"{Email}\" | \"{Nationality}\" \"{Country}\" \"{Gender}\" \"{MaritalStatus}\" | \"{Faculty}\" \"{Specialty}\" \"{SpecialtyCode}\"";
        }
    }
}
