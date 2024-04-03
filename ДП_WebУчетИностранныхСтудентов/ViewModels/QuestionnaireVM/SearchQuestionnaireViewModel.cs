namespace MonitoringTheProgressOfForeignStudents.ViewModels.QuestionnaireVM
{
    public class SearchQuestionnaireViewModel
    {
        public Guid? GenderId { get; set; }
        public Guid? SpecialtyId { get; set; }
        public Guid? CountryId { get; set; }
        public Guid? NationalityId { get; set; }
        public Guid? MaritalStatusId { get; set; }
        public Guid? EducationLanguageId { get; set; }
        public Guid? EducationTypeId { get; set; }
        public string SearchString { get; set; }
    }
}
