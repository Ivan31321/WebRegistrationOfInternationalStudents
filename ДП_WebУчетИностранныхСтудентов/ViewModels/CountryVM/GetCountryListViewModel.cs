namespace MonitoringTheProgressOfForeignStudents.ViewModels.CountryVM
{
    public class GetCountryListViewModel
    {
        public SearchCountryViewModel SearchCountry { get; set; }
        public IEnumerable<GetCountryViewModel> CountryViewModels { get; set; }
    }
}
