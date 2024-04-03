using Microsoft.EntityFrameworkCore;
using MonitoringTheProgressOfForeignStudents.Application.Interfaces.Repositories;
using MonitoringTheProgressOfForeignStudents.Domain.Model;

namespace MonitoringTheProgressOfForeignStudents.Infrastructure.Repositories
{
    public class FacultyRepository : GenericRepository<Faculty>, IFacultyRepository
    {
        public FacultyRepository(AppDbContext context) : base(context)
        {
        }

        public IQueryable<Faculty> GetAllFull()
        {
            return base.GetAll().Include(x=>x.Specialties);
        }

        public async Task<Faculty?> GetFullByIdAsync(Guid id)
        {
            return await GetAllFull().FirstOrDefaultAsync(x=>x.Id == id);
        }
    }
}
