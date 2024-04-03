using MonitoringTheProgressOfForeignStudents.Domain.Model;
using MonitoringTheProgressOfForeignStudents.ViewModels.Abstract;

namespace MonitoringTheProgressOfForeignStudents.ViewModels.NationalityVM
{
    public class EditNationalityVMConverter : ConverterVM<EditNationalityViewModel, Nationality>
    {
        public override EditNationalityViewModel ConvertFrom(Nationality obj)
        {
            return new EditNationalityViewModel
            {
                Name = obj.Name,
            };
        }

        public override Nationality ConvertTo(EditNationalityViewModel obj)
        {
            return new Nationality
            {
                Name = obj.Name,
            };
        }

        public override Nationality Update(EditNationalityViewModel from, Nationality to)
        {
            to.Name = from.Name;
            return to;
        }
    }
}
