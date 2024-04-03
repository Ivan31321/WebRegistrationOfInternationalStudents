using Microsoft.EntityFrameworkCore;
using MonitoringTheProgressOfForeignStudents.Application.Interfaces.Repositories;
using MonitoringTheProgressOfForeignStudents.Domain.Model;

namespace MonitoringTheProgressOfForeignStudents.Infrastructure.Repositories
{
    public class EducationLanguageRepository : GenericRepository<EducationLanguage>, IEducationLanguageRepository
    {
        public EducationLanguageRepository(AppDbContext context) : base(context)
        {
        }

        public IQueryable<EducationLanguage> GetAllFull()
        {
            return base.GetAll().Include(x=>x.Questionnaires);
        }

        public async Task<EducationLanguage?> GetFullByIdAsync(Guid id)
        {
            return await GetAllFull().FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
