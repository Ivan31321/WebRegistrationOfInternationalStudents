using MonitoringTheProgressOfForeignStudents.Domain.Model;
using MonitoringTheProgressOfForeignStudents.ViewModels.Abstract;

namespace MonitoringTheProgressOfForeignStudents.ViewModels.SpecialtyVM
{
    public class EditSpecialtyVMConverter : ConverterVM<EditSpecialtyViewModel, Specialty>
    {
        public override EditSpecialtyViewModel ConvertFrom(Specialty obj)
        {
            return new EditSpecialtyViewModel
            {
                Code = obj.Code,
                FacultyId = obj.FacultyId,
                Name = obj.Name,
            };
        }

        public override Specialty ConvertTo(EditSpecialtyViewModel obj)
        {
            return new Specialty
            {
                Code = obj.Code,
                Name = obj.Name,
                FacultyId = obj.FacultyId
            };
        }

        public override Specialty Update(EditSpecialtyViewModel from, Specialty to)
        {
            to.Code = from.Code;
            to.Name = from.Name;
            to.FacultyId = from.FacultyId;
            return to;
        }
    }
}
