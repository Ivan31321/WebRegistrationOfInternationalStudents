using Microsoft.EntityFrameworkCore;
using MonitoringTheProgressOfForeignStudents.Application.Interfaces.Repositories;
using MonitoringTheProgressOfForeignStudents.Domain.Model;

namespace MonitoringTheProgressOfForeignStudents.Infrastructure.Repositories
{
    internal class HomeRepository : GenericRepository<Home>, IHomeRepository
    {
        public HomeRepository(AppDbContext context) : base(context)
        {
        }

        public IQueryable<Home> GetAllFull()
        {
            return base.GetAll();
        }

        public async Task<Home?> GetFullByIdAsync(Guid id)
        {
            return await GetAllFull().FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
