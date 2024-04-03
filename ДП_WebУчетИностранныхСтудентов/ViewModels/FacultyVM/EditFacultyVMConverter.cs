using MonitoringTheProgressOfForeignStudents.Domain.Model;
using MonitoringTheProgressOfForeignStudents.ViewModels.Abstract;

namespace MonitoringTheProgressOfForeignStudents.ViewModels.FacultyVM
{
    public class EditFacultyVMConverter : ConverterVM<EditFacultyViewModel, Faculty>
    {
        public override EditFacultyViewModel ConvertFrom(Faculty obj)
        {
            return new EditFacultyViewModel
            {
                Name = obj.Name,
            };
        }

        public override Faculty ConvertTo(EditFacultyViewModel obj)
        {
            return new Faculty
            {
                Name = obj.Name,
            };
        }

        public override Faculty Update(EditFacultyViewModel from, Faculty to)
        {
            to.Name = from.Name;
            return to;
        }
    }
}
