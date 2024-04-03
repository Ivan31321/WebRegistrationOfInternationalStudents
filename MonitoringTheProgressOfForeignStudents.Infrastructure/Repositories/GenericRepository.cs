using Microsoft.EntityFrameworkCore;
using MonitoringTheProgressOfForeignStudents.Application.Interfaces.Repositories;
using MonitoringTheProgressOfForeignStudents.Domain.Model;

namespace MonitoringTheProgressOfForeignStudents.Infrastructure.Repositories
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected internal AppDbContext Context { get; }
        protected internal DbSet<T> Set { get; }

        public GenericRepository(AppDbContext context)
        {
            Context = context;
            Set = context.Set<T>();
        }

        public async Task<Guid> CreateAsync(T entity)
        {
            var addedEntity = await Set.AddAsync(entity);
            await Context.SaveChangesAsync();

            return addedEntity.Entity.Id;
        }

        public async Task DeleteAsync(T entity)
        {
            Set.Remove(entity);
            await Context.SaveChangesAsync();
        }

        public IQueryable<T> GetAll()
        {
            return Set.AsQueryable();
        }

        public async Task<T?> GetById(Guid id)
        {
            return await Set.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Guid> UpdateAsync(T entity)
        {
            var updatedEntity = Set.Update(entity);
            await Context.SaveChangesAsync();

            return updatedEntity.Entity.Id;
        }

        public async Task DeleteRange(IEnumerable<T> entities)
        {
            Set.RemoveRange(entities);
            await Context.SaveChangesAsync();
        }
    }
}
