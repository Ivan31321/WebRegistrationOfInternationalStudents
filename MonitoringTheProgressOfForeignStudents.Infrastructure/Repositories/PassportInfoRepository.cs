using Microsoft.EntityFrameworkCore;
using MonitoringTheProgressOfForeignStudents.Application.Interfaces.Repositories;
using MonitoringTheProgressOfForeignStudents.Domain.Model;

namespace MonitoringTheProgressOfForeignStudents.Infrastructure.Repositories
{
    public class PassportInfoRepository : GenericRepository<PassportInfo>, IPassportInfoRepository
    {
        public PassportInfoRepository(AppDbContext context) : base(context)
        {
        }

        public IQueryable<PassportInfo> GetAllFull()
        {
            return base.GetAll();
        }

        public async Task<PassportInfo?> GetFullByIdAsync(Guid id)
        {
            return await GetAllFull().FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
