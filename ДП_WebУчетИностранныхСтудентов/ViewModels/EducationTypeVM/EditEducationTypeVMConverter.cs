using MonitoringTheProgressOfForeignStudents.Domain.Model;
using MonitoringTheProgressOfForeignStudents.ViewModels.Abstract;

namespace MonitoringTheProgressOfForeignStudents.ViewModels.EducationTypeVM
{
    public class EditEducationTypeVMConverter : ConverterVM<EditEducationTypeViewModel, EducationType>
    {
        public override EditEducationTypeViewModel ConvertFrom(EducationType obj)
        {
            return new EditEducationTypeViewModel
            {
                Type = obj.Type,
            };
        }

        public override EducationType ConvertTo(EditEducationTypeViewModel obj)
        {
            return new EducationType
            {
                Type = obj.Type,
            };
        }

        public override EducationType Update(EditEducationTypeViewModel from, EducationType to)
        {
            to.Type = from.Type;
            return to;
        }
    }
}
