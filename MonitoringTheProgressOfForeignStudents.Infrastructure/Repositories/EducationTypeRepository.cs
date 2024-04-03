using Microsoft.EntityFrameworkCore;
using MonitoringTheProgressOfForeignStudents.Application.Interfaces.Repositories;
using MonitoringTheProgressOfForeignStudents.Domain.Model;

namespace MonitoringTheProgressOfForeignStudents.Infrastructure.Repositories
{
    public class EducationTypeRepository : GenericRepository<EducationType>, IEducationTypeRepository
    {
        public EducationTypeRepository(AppDbContext context) : base(context)
        {
        }

        public IQueryable<EducationType> GetAllFull()
        {
            return base.GetAll().Include(x=>x.Questionnaires);
        }

        public async Task<EducationType?> GetFullByIdAsync(Guid id)
        {
            return await GetAllFull().FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
