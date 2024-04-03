using Microsoft.Win32;
using MonitoringTheProgressOfForeignStudents.Domain.Model;
using MonitoringTheProgressOfForeignStudents.ViewModels.Abstract;

namespace MonitoringTheProgressOfForeignStudents.ViewModels.StudentCardVM
{
    public class EditStudentCardVMConverter : ConverterVM<EditStudentCardViewModel, StudentCard>
    {
        public override EditStudentCardViewModel ConvertFrom(StudentCard obj)
        {
            return new EditStudentCardViewModel
            {
                PersonalDetailsId = obj.PersonalDetails.Id,
                Name = obj.PersonalDetails.Name,
                Surname = obj.PersonalDetails.Surname,
                Patronymic = obj.PersonalDetails.Patronymic,
                Birthday = obj.PersonalDetails.Birthday,
                Gender = obj.PersonalDetails.Gender?.Name,
                Country = obj.PersonalDetails.Country?.Name,
                Faculty = obj.PersonalDetails.QuestionnaireLatest.Specialty?.Faculty?.Name,
                PlaceofBirth = obj.PersonalDetails.PlaceofBirth,
                Homes = obj.PersonalDetails.QuestionnaireLatest.Homes.ToList(),
                Passports = obj.Passports != null ? obj.Passports.OrderByDescending(x=>x.Created).ToList() : new List<PassportInfo>(),
                Registers = obj.Registers != null ? obj.Registers.OrderByDescending(x => x.Created).ToList() : new List<Register>(),
            };
        }

        public override StudentCard ConvertTo(EditStudentCardViewModel obj)
        {
            return new StudentCard
            {
                PersonalDetailsId = obj.PersonalDetailsId,
                Passports = obj.Passports ?? new List<PassportInfo>(),
                Registers = obj.Registers ?? new List<Register>(),
            };
        }

        public override StudentCard Update(EditStudentCardViewModel from, StudentCard to)
        {
            if (from.Passports == null) from.Passports = new List<PassportInfo>();
            if (from.Registers == null) from.Registers = new List<Register>();

            to.Passports = from.Passports;
            to.Registers = from.Registers;

            return to;
        }
    }
}
