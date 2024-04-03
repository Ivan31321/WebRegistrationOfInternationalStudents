using Microsoft.EntityFrameworkCore;
using MonitoringTheProgressOfForeignStudents.Application.Interfaces.Repositories;
using MonitoringTheProgressOfForeignStudents.Domain.Model;

namespace MonitoringTheProgressOfForeignStudents.Infrastructure.Repositories
{
    public class QuestionnaireRepository : GenericRepository<Questionnaire>, IQuestionnaireRepository
    {
        public QuestionnaireRepository(AppDbContext context) : base(context)
        {
        }

        public IQueryable<Questionnaire> GetAllFull()
        {
            return base.GetAll()
                .Include(x => x.EducationLanguage)
                .Include(x => x.Specialty)
                    .ThenInclude(x => x.Faculty)
                .Include(x => x.EducationType)
                .Include(x => x.PersonalDetails)
                    .ThenInclude(x => x.MaritalStatus)
                .Include(x => x.PersonalDetails)
                    .ThenInclude(x => x.Gender)
                .Include(x => x.PersonalDetails)
                    .ThenInclude(x => x.Country)
                .Include(x => x.PersonalDetails)
                    .ThenInclude(x => x.Nationality)
                .Include(x => x.Homes)
                .Include(x => x.Orders);
        }

        public async Task<Questionnaire?> GetFullByIdAsync(Guid id)
        {
            return await GetAllFull().FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
