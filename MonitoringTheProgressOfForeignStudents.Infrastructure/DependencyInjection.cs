using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MonitoringTheProgressOfForeignStudents.Application.Interfaces.Repositories;
using MonitoringTheProgressOfForeignStudents.Infrastructure.Repositories;

namespace MonitoringTheProgressOfForeignStudents.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection ImplementAppPersistence(this
            IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)), ServiceLifetime.Transient);
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            return services;
        }

        public static IServiceCollection ImplementAuthPersistence(this
            IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AuthDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("AuthConnection"),
                b => b.MigrationsAssembly(typeof(AuthDbContext).Assembly.FullName)), ServiceLifetime.Transient);

            services.AddScoped<AuthDbContext>();

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ICountryRepository, CountryRepository>()
                .AddScoped<IFacultyRepository, FacultyRepository>()
                .AddScoped<IGenderRepository, GenderRepository>()
                .AddScoped<IMaritalStatusRepository, MaritalStatusRepository>()
                .AddScoped<INationalityRepository, NationalityRepository>()
                .AddScoped<IPersonalDetailsRepository, PersonalDetailsRepository>()
                .AddScoped<IQuestionnaireRepository, QuestionnaireRepository>()
                .AddScoped<ISpecialtyRepository, SpecialtyRepository>()
                .AddScoped<IEducationLanguageRepository, EducationLanguageRepository>()
                .AddScoped<IEducationTypeRepository, EducationTypeRepository>()
                .AddScoped<IOrderRepository, OrderRepository>()
                .AddScoped<IStudentCardRepository, StudentCardRepository>()
                .AddScoped<IHomeRepository, HomeRepository>()
                .AddScoped<IRegisterRepository, RegisterRepository>()
                .AddScoped<IPassportInfoRepository, PassportInfoRepository>();

            return services;
        }
    }
}
