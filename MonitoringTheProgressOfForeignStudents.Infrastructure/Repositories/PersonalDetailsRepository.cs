using Microsoft.EntityFrameworkCore;
using MonitoringTheProgressOfForeignStudents.Application.Interfaces.Repositories;
using MonitoringTheProgressOfForeignStudents.Domain.Model;

namespace MonitoringTheProgressOfForeignStudents.Infrastructure.Repositories
{
    public class PersonalDetailsRepository : GenericRepository<PersonalDetails>, IPersonalDetailsRepository
    {
        public PersonalDetailsRepository(AppDbContext context) : base(context)
        {
        }

        public IQueryable<PersonalDetails> GetAllFull()
        {
            return base.GetAll()
                .Include(x => x.Gender)
                .Include(x => x.Country)
                .Include(x => x.Nationality)
                .Include(x => x.MaritalStatus)
                .Include(p => p.Questionnaires)
                    .ThenInclude(q => q.Specialty)
                        .ThenInclude(s => s.Faculty)
                .Include(p => p.Questionnaires)
                    .ThenInclude(q => q.Homes);
        }

        public async Task<PersonalDetails?> GetFullByIdAsync(Guid id)
        {
            return await GetAllFull().FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
