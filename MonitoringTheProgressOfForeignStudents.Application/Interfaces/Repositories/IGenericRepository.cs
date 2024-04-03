using MonitoringTheProgressOfForeignStudents.Domain.Model;

namespace MonitoringTheProgressOfForeignStudents.Application.Interfaces.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        IQueryable<TEntity> GetAll();
        Task<TEntity?> GetById(Guid id);
        Task<Guid> CreateAsync(TEntity entity);
        Task<Guid> UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task DeleteRange(IEnumerable<TEntity> entities);
    }
}
