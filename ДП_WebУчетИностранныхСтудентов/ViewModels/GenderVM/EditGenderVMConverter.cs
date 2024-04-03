using MonitoringTheProgressOfForeignStudents.Domain.Model;
using MonitoringTheProgressOfForeignStudents.ViewModels.Abstract;

namespace MonitoringTheProgressOfForeignStudents.ViewModels.GenderVM
{
    public class EditGenderVMConverter : ConverterVM<EditGenderViewModel, Gender>
    {
        public override EditGenderViewModel ConvertFrom(Gender obj)
        {
            return new EditGenderViewModel
            {
                Name = obj.Name,
            };
        }

        public override Gender ConvertTo(EditGenderViewModel obj)
        {
            return new Gender
            {
                Name = obj.Name,
            };
        }

        public override Gender Update(EditGenderViewModel from, Gender to)
        {
            to.Name = from.Name;
            return to;
        }
    }
}
