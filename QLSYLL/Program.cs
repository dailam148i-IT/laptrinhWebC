using Microsoft.EntityFrameworkCore;
using QLSYLL.Infrastructure.Extensions;
using QLSYLL.Infrastructure.RequestContext;
using QLSYLL.Infrastructure.Services;
using QLSYLL.Models;

namespace QLSYLL;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<ICurrentRequestContext, HttpCurrentRequestContext>();
        builder.Services.AddScoped<AuditLogger>();

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        // Thêm Session
        builder.Services.AddDistributedMemoryCache();
        builder.Services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(30);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.EnsureCreated();
            dbContext.Database.ExecuteSqlRaw(
                """
                IF OBJECT_ID(N'[EmployeeDocuments]', N'U') IS NULL
                BEGIN
                    CREATE TABLE [EmployeeDocuments](
                        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
                        [EmployeeId] INT NOT NULL,
                        [Title] NVARCHAR(200) NOT NULL,
                        [Category] NVARCHAR(50) NOT NULL,
                        [FileName] NVARCHAR(255) NOT NULL,
                        [FilePath] NVARCHAR(255) NOT NULL,
                        [ContentType] NVARCHAR(100) NULL,
                        [FileSize] BIGINT NOT NULL,
                        [UploadedByUserId] INT NULL,
                        [CreatedAt] DATETIME2 NOT NULL,
                        CONSTRAINT [FK_EmployeeDocuments_Employees_EmployeeId]
                            FOREIGN KEY ([EmployeeId]) REFERENCES [Employees]([Id]) ON DELETE CASCADE
                    );
                    CREATE INDEX [IX_EmployeeDocuments_EmployeeId] ON [EmployeeDocuments]([EmployeeId]);
                END
                """);
        }

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
        }
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();

        // Bật Session middleware
        app.UseSession();

        app.Use(async (context, next) =>
        {
            if (string.IsNullOrWhiteSpace(context.Session.GetString("UserId")) &&
                context.Request.Cookies.TryGetValue("remember_me", out var userIdRaw) &&
                int.TryParse(userIdRaw, out var userId))
            {
                using var scope = context.RequestServices.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var user = await dbContext.Users
                    .Include(x => x.Role)
                    .FirstOrDefaultAsync(x => x.Id == userId && !x.IsDeleted);

                if (user is not null && user.IsActive)
                {
                    context.SignIn(user);
                }
            }

            await next();
        });

        app.UseAuthorization();

        app.MapStaticAssets();
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Account}/{action=Login}/{id?}")
            .WithStaticAssets();

        app.Run();
    }
}
