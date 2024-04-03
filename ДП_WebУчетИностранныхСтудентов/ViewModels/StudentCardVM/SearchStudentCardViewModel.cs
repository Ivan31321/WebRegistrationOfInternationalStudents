namespace MonitoringTheProgressOfForeignStudents.ViewModels.StudentCardVM
{
    public class SearchStudentCardViewModel
    {
        public Guid GenderId { get; set; }
        public Guid CountryId { get; set; }
        public Guid NationalityId { get; set; }
        public Guid MaritalStatusId { get; set; }
        public bool? PassportValid { get; set; }
        public bool? RegisterValid { get; set; }
        public string SearchString { get; set; }
    }
}
