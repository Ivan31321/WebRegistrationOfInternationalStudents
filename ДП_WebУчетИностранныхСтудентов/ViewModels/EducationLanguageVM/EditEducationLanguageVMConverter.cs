using MonitoringTheProgressOfForeignStudents.Domain.Model;
using MonitoringTheProgressOfForeignStudents.ViewModels.Abstract;

namespace MonitoringTheProgressOfForeignStudents.ViewModels.EducationLanguageVM
{
    public class EditEducationLanguageVMConverter : ConverterVM<EditEducationLanguageViewModel, EducationLanguage>
    {
        public override EditEducationLanguageViewModel ConvertFrom(EducationLanguage obj)
        {
            return new EditEducationLanguageViewModel
            {
                Language = obj.Language,
            };
        }

        public override EducationLanguage ConvertTo(EditEducationLanguageViewModel obj)
        {
            return new EducationLanguage
            {
                Language = obj.Language,
            };
        }

        public override EducationLanguage Update(EditEducationLanguageViewModel from, EducationLanguage to)
        {
            to.Language = from.Language;
            return to;
        }
    }
}
