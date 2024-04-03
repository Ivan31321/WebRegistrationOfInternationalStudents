using Microsoft.EntityFrameworkCore;
using MonitoringTheProgressOfForeignStudents.Domain.Model;

namespace MonitoringTheProgressOfForeignStudents.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) :
            base(options)
        { Database.EnsureCreated(); }

        public DbSet<Country> Countries { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<MaritalStatus> MaritalStatuses { get; set; }
        public DbSet<Nationality> Nationalities { get; set; }
        public DbSet<PersonalDetails> PersonalDetails { get; set; }
        public DbSet<Questionnaire> Questionnaires { get; set; }
        public DbSet<Specialty> Specialties { get; set; }
        public DbSet<EducationLanguage> EducationLanguages { get; set; }
        public DbSet<EducationType> EducationTypes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Home> Homes { get; set; }
        public DbSet<StudentCard> StudentCards { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity && (
                        e.State == EntityState.Added));

            foreach (var entityEntry in entries)
            {
                if (entityEntry.State == EntityState.Added)
                {
                    if (((BaseEntity)entityEntry.Entity).Created == null)
                        ((BaseEntity)entityEntry.Entity).Created = DateTime.Now;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Questionnaire>().HasMany(x => x.Orders).WithOne(x => x.Questionnaire).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Questionnaire>().HasMany(x => x.Homes).WithOne(x => x.Questionnaire).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<StudentCard>().HasMany(x => x.Registers).WithOne(x => x.StudentCard).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<StudentCard>().HasMany(x => x.Passports).WithOne(x => x.StudentCard).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Faculty>().HasMany(x => x.Specialties).WithOne(x=>x.Faculty).OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Country>().HasMany(x=>x.Persons).WithOne(x=>x.Country).OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Nationality>().HasMany(x => x.Persons).WithOne(x => x.Nationality).OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<MaritalStatus>().HasMany(x => x.Persons).WithOne(x => x.MaritalStatus).OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Gender>().HasMany(x => x.Persons).WithOne(x => x.Gender).OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<EducationType>().HasMany(x => x.Questionnaires).WithOne(x => x.EducationType).OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<EducationLanguage>().HasMany(x => x.Questionnaires).WithOne(x => x.EducationLanguage).OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Specialty>().HasMany(x => x.Questionnaires).WithOne(x => x.Specialty).OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<PersonalDetails>().HasMany(x => x.StudentCards).WithOne(x => x.PersonalDetails).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<PersonalDetails>().HasMany(x => x.Questionnaires).WithOne(x => x.PersonalDetails).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
