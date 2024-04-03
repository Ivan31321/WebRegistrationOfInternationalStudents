using MonitoringTheProgressOfForeignStudents.Domain.Model;
using MonitoringTheProgressOfForeignStudents.ViewModels.Abstract;

namespace MonitoringTheProgressOfForeignStudents.ViewModels.CountryVM
{
    public class EditCountryVMConverter : ConverterVM<EditCountryViewModel, Country>
    {
        public override EditCountryViewModel ConvertFrom(Country obj)
        {
            return new EditCountryViewModel
            {
                Name = obj.Name,
            };
        }

        public override Country ConvertTo(EditCountryViewModel obj)
        {
            return new Country()
            {
                Name = obj.Name,
            };
        }

        public override Country Update(EditCountryViewModel from, Country to)
        {
            to.Name = from.Name;
            return to;
        }
    }
}
