using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using QLSYLL.Infrastructure.Data;
using QLSYLL.Infrastructure.RequestContext;

namespace QLSYLL.Models
{
    public class ApplicationDbContext : DbContext
    {
        private readonly ICurrentRequestContext _currentRequestContext;

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            ICurrentRequestContext currentRequestContext)
            : base(options)
        {
            _currentRequestContext = currentRequestContext;
        }

        public DbSet<Role> Roles => Set<Role>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Department> Departments => Set<Department>();
        public DbSet<Position> Positions => Set<Position>();
        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<Skill> Skills => Set<Skill>();
        public DbSet<EmployeeSkill> EmployeeSkills => Set<EmployeeSkill>();
        public DbSet<EmployeeEducation> EmployeeEducations => Set<EmployeeEducation>();
        public DbSet<FamilyMember> FamilyMembers => Set<FamilyMember>();
        public DbSet<WorkHistory> WorkHistories => Set<WorkHistory>();
        public DbSet<EmployeeDocument> EmployeeDocuments => Set<EmployeeDocument>();
        public DbSet<Announcement> Announcements => Set<Announcement>();
        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(x => x.Username).IsUnique();
                entity.HasIndex(x => x.Email).IsUnique();
                entity.HasOne(x => x.Role)
                    .WithMany(x => x.Users)
                    .HasForeignKey(x => x.RoleId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasIndex(x => x.Code).IsUnique();
                entity.HasOne(x => x.Manager)
                    .WithMany()
                    .HasForeignKey(x => x.ManagerId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasOne(x => x.User)
                    .WithOne(x => x.Employee)
                    .HasForeignKey<Employee>(x => x.UserId);
                entity.Property(x => x.Salary).HasPrecision(18, 2);
            });

            modelBuilder.Entity<Position>(entity =>
            {
                entity.Property(x => x.BaseSalary).HasPrecision(18, 2);
            });

            modelBuilder.Entity<EmployeeSkill>(entity =>
            {
                entity.HasKey(x => new { x.EmployeeId, x.SkillId });
            });

            modelBuilder.Entity<EmployeeEducation>(entity =>
            {
                entity.HasOne(x => x.Employee)
                    .WithMany(x => x.EmployeeEducations)
                    .HasForeignKey(x => x.EmployeeId);
            });

            modelBuilder.Entity<WorkHistory>(entity =>
            {
                entity.HasOne(x => x.Employee)
                    .WithMany(x => x.WorkHistories)
                    .HasForeignKey(x => x.EmployeeId);
            });

            modelBuilder.Entity<FamilyMember>(entity =>
            {
                entity.HasOne(x => x.Employee)
                    .WithMany(x => x.FamilyMembers)
                    .HasForeignKey(x => x.EmployeeId);
            });

            modelBuilder.Entity<EmployeeDocument>(entity =>
            {
                entity.HasOne(x => x.Employee)
                    .WithMany(x => x.EmployeeDocuments)
                    .HasForeignKey(x => x.EmployeeId);
            });

            modelBuilder.Entity<Announcement>(entity =>
            {
                entity.HasOne(x => x.AuthorUser)
                    .WithMany()
                    .HasForeignKey(x => x.AuthorUserId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<AuditLog>(entity =>
            {
                entity.HasOne(x => x.User)
                    .WithMany()
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Role>().HasData(SeedData.Roles);
            modelBuilder.Entity<User>().HasData(SeedData.Users);
            modelBuilder.Entity<Department>().HasData(SeedData.Departments);
            modelBuilder.Entity<Position>().HasData(SeedData.Positions);
            modelBuilder.Entity<Employee>().HasData(SeedData.Employees);
            modelBuilder.Entity<Skill>().HasData(SeedData.Skills);
            modelBuilder.Entity<EmployeeSkill>().HasData(SeedData.EmployeeSkills);
            modelBuilder.Entity<EmployeeEducation>().HasData(SeedData.EmployeeEducations);
            modelBuilder.Entity<FamilyMember>().HasData(SeedData.FamilyMembers);
            modelBuilder.Entity<WorkHistory>().HasData(SeedData.WorkHistories);
            modelBuilder.Entity<Announcement>().HasData(SeedData.Announcements);
        }

        public override int SaveChanges()
        {
            return SaveChangesAsync().GetAwaiter().GetResult();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var auditEntries = CreateAuditEntries();
            var result = await base.SaveChangesAsync(cancellationToken);

            if (auditEntries.Count > 0)
            {
                AuditLogs.AddRange(auditEntries);
                result += await base.SaveChangesAsync(cancellationToken);
            }

            return result;
        }

        private List<AuditLog> CreateAuditEntries()
        {
            ChangeTracker.DetectChanges();

            var entries = ChangeTracker.Entries()
                .Where(entry => entry.Entity is not AuditLog &&
                                entry.State is EntityState.Added or EntityState.Modified or EntityState.Deleted)
                .ToList();

            var logs = new List<AuditLog>();

            foreach (var entry in entries)
            {
                var oldValues = new Dictionary<string, object?>();
                var newValues = new Dictionary<string, object?>();

                foreach (var property in entry.Properties)
                {
                    if (property.Metadata.IsPrimaryKey())
                    {
                        continue;
                    }

                    if (entry.State == EntityState.Added)
                    {
                        newValues[property.Metadata.Name] = property.CurrentValue;
                    }
                    else if (entry.State == EntityState.Deleted)
                    {
                        oldValues[property.Metadata.Name] = property.OriginalValue;
                    }
                    else if (property.IsModified)
                    {
                        oldValues[property.Metadata.Name] = property.OriginalValue;
                        newValues[property.Metadata.Name] = property.CurrentValue;
                    }
                }

                if (entry.State == EntityState.Modified && oldValues.Count == 0 && newValues.Count == 0)
                {
                    continue;
                }

                logs.Add(new AuditLog
                {
                    TableName = entry.Metadata.GetTableName() ?? entry.Entity.GetType().Name,
                    RecordId = entry.Properties.FirstOrDefault(x => x.Metadata.IsPrimaryKey())?.CurrentValue?.ToString()
                        ?? entry.Properties.FirstOrDefault(x => x.Metadata.IsPrimaryKey())?.OriginalValue?.ToString(),
                    Action = entry.State switch
                    {
                        EntityState.Added => "INSERT",
                        EntityState.Modified => "UPDATE",
                        EntityState.Deleted => "DELETE",
                        _ => "UNKNOWN"
                    },
                    OldValues = oldValues.Count == 0 ? null : JsonSerializer.Serialize(oldValues),
                    NewValues = newValues.Count == 0 ? null : JsonSerializer.Serialize(newValues),
                    UserId = _currentRequestContext.UserId,
                    IpAddress = _currentRequestContext.IpAddress,
                    Timestamp = DateTime.UtcNow
                });
            }

            return logs;
        }
    }
}
