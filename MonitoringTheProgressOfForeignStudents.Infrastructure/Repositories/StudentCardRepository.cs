using Microsoft.EntityFrameworkCore;
using MonitoringTheProgressOfForeignStudents.Application.Interfaces.Repositories;
using MonitoringTheProgressOfForeignStudents.Domain.Model;

namespace MonitoringTheProgressOfForeignStudents.Infrastructure.Repositories
{
    public class StudentCardRepository : GenericRepository<StudentCard>, IStudentCardRepository
    {
        public StudentCardRepository(AppDbContext context) : base(context)
        {
        }

        public IQueryable<StudentCard> GetAllFull()
        {
            return base.GetAll()
                .Include(x => x.PersonalDetails)
                    .ThenInclude(x => x.Country)
                .Include(x => x.PersonalDetails)
                    .ThenInclude(x => x.Nationality)
                .Include(x => x.PersonalDetails)
                    .ThenInclude(x => x.Gender)
                .Include(x => x.PersonalDetails)
                    .ThenInclude(x => x.MaritalStatus)
                .Include(x => x.PersonalDetails)
                    .ThenInclude(x => x.Questionnaires)
                        .ThenInclude(x => x.Homes)
                .Include(x => x.PersonalDetails)
                    .ThenInclude(x => x.Questionnaires)
                        .ThenInclude(x => x.Specialty)
                            .ThenInclude(x => x.Faculty)
                .Include(x => x.Passports)
                .Include(x => x.Registers);
        }

        public async Task<StudentCard?> GetFullByIdAsync(Guid id)
        {
            return await GetAllFull().FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
