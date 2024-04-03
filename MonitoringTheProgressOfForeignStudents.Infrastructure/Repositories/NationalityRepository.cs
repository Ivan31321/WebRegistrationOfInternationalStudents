using Microsoft.EntityFrameworkCore;
using MonitoringTheProgressOfForeignStudents.Application.Interfaces.Repositories;
using MonitoringTheProgressOfForeignStudents.Domain.Model;

namespace MonitoringTheProgressOfForeignStudents.Infrastructure.Repositories
{
    public class NationalityRepository : GenericRepository<Nationality>, INationalityRepository
    {
        public NationalityRepository(AppDbContext context) : base(context)
        {
        }

        public IQueryable<Nationality> GetAllFull()
        {
            return base.GetAll().Include(x=>x.Persons);
        }

        public async Task<Nationality?> GetFullByIdAsync(Guid id)
        {
            return await GetAllFull().FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
