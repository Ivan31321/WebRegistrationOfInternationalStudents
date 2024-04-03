namespace MonitoringTheProgressOfForeignStudents.ViewModels.PersonalDetailsVM
{
    public class SearchPersonalDetailsViewModel
    {
        public Guid? GenderId { get; set; }
        public Guid? CountryId { get; set; }
        public Guid? NationalityId { get; set; }
        public Guid? MaritalStatusId { get; set; }
        public string SearchString { get; set; }
    }
}
