using Microsoft.EntityFrameworkCore;
using MonitoringTheProgressOfForeignStudents.Application.Interfaces.Repositories;
using MonitoringTheProgressOfForeignStudents.Domain.Model;

namespace MonitoringTheProgressOfForeignStudents.Infrastructure.Repositories
{
    public class MaritalStatusRepository : GenericRepository<MaritalStatus>, IMaritalStatusRepository
    {
        public MaritalStatusRepository(AppDbContext context) : base(context)
        {
        }

        public IQueryable<MaritalStatus> GetAllFull()
        {
            return base.GetAll().Include(x=>x.Persons);
        }

        public async Task<MaritalStatus?> GetFullByIdAsync(Guid id)
        {
            return await GetAllFull().FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
