using MonitoringTheProgressOfForeignStudents.Domain.Model;

namespace MonitoringTheProgressOfForeignStudents.Application.Interfaces.Repositories
{
    public interface IRepository<TEntity>: IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        public IQueryable<TEntity> GetAllFull();
        public Task<TEntity?> GetFullByIdAsync(Guid id);
    }
}
