using MonitoringTheProgressOfForeignStudents.ViewModels.CountryVM;
using MonitoringTheProgressOfForeignStudents.ViewModels.EducationLanguageVM;
using MonitoringTheProgressOfForeignStudents.ViewModels.EducationTypeVM;
using MonitoringTheProgressOfForeignStudents.ViewModels.FacultyVM;
using MonitoringTheProgressOfForeignStudents.ViewModels.GenderVM;
using MonitoringTheProgressOfForeignStudents.ViewModels.MaritalStatusVM;
using MonitoringTheProgressOfForeignStudents.ViewModels.NationalityVM;
using MonitoringTheProgressOfForeignStudents.ViewModels.PersonalDetailsVM;
using MonitoringTheProgressOfForeignStudents.ViewModels.QuestionnaireVM;
using MonitoringTheProgressOfForeignStudents.ViewModels.SpecialtyVM;
using MonitoringTheProgressOfForeignStudents.ViewModels.StudentCardVM;

namespace MonitoringTheProgressOfForeignStudents.ViewModels
{
    public static class DependecyInjection
    {
        public static IServiceCollection AddConverters(this
            IServiceCollection services)
        {
            services.AddSingleton<EditCountryVMConverter>()
                .AddSingleton<EditEducationLanguageVMConverter>()
                .AddSingleton<EditEducationTypeVMConverter>()
                .AddSingleton<EditFacultyVMConverter>()
                .AddSingleton<EditGenderVMConverter>()
                .AddSingleton<EditNationalityVMConverter>()
                .AddSingleton<EditMaritalStatusVMConverter>()
                .AddSingleton<EditPersonalDetailsVMConverter>()
                .AddSingleton<EditQuestionnaireVMConverter>()
                .AddSingleton<EditSpecialtyVMConverter>()
                .AddSingleton<EditStudentCardVMConverter>();

            return services;
        }
    }
}
