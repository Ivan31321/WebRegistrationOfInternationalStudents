using Microsoft.EntityFrameworkCore;
using MonitoringTheProgressOfForeignStudents.Application.Interfaces.Repositories;
using MonitoringTheProgressOfForeignStudents.Domain.Model;

namespace MonitoringTheProgressOfForeignStudents.Infrastructure.Repositories
{
    public class RegisterRepository : GenericRepository<Register>, IRegisterRepository
    {
        public RegisterRepository(AppDbContext context) : base(context)
        {
        }

        public IQueryable<Register> GetAllFull()
        {
            return base.GetAll();
        }

        public async Task<Register?> GetFullByIdAsync(Guid id)
        {
            return await GetAllFull().FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
