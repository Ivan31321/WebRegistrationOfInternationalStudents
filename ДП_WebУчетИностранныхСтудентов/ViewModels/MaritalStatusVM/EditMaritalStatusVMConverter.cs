using MonitoringTheProgressOfForeignStudents.Domain.Model;
using MonitoringTheProgressOfForeignStudents.ViewModels.Abstract;

namespace MonitoringTheProgressOfForeignStudents.ViewModels.MaritalStatusVM
{
    public class EditMaritalStatusVMConverter : ConverterVM<EditMaritalStatusViewModel, MaritalStatus>
    {
        public override EditMaritalStatusViewModel ConvertFrom(MaritalStatus obj)
        {
            return new EditMaritalStatusViewModel
            {
                Name = obj.Name,
            };
        }

        public override MaritalStatus ConvertTo(EditMaritalStatusViewModel obj)
        {
            return new MaritalStatus
            {
                Name = obj.Name
            };
        }

        public override MaritalStatus Update(EditMaritalStatusViewModel from, MaritalStatus to)
        {
            to.Name = from.Name;
            return to;
        }
    }
}
