using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using QLSYLL.Infrastructure.Security;
using QLSYLL.Models;
using QLSYLL.Models.ViewModels;

namespace QLSYLL.Infrastructure.Services;

public class AccountProvisioningService(ApplicationDbContext dbContext, AuditLogger auditLogger) : IAccountProvisioningService
{
    public async Task<AccountProvisioningResult> CreateAdminAccountAsync(
        AdminAccountCreateViewModel model,
        CancellationToken cancellationToken = default)
    {
        var username = NormalizeRequired(model.Username);
        var email = NormalizeRequired(model.Email);
        var fullName = NormalizeRequired(model.FullName);

        if (!RoleNames.IsAdminRole(model.Role))
        {
            return AccountProvisioningResult.Failure("Chỉ được tạo tài khoản quản trị SA hoặc HR tại màn hình này.");
        }

        if (await UserExistsAsync(username, email, cancellationToken: cancellationToken))
        {
            return AccountProvisioningResult.Failure("Tên đăng nhập hoặc email đã tồn tại.");
        }

        var roleId = await GetRoleIdAsync(model.Role, cancellationToken);
        if (roleId == 0)
        {
            return AccountProvisioningResult.Failure("Không tìm thấy vai trò hợp lệ.");
        }

        var user = new User
        {
            Username = username,
            FullName = fullName,
            Email = email,
            RoleId = roleId,
            PasswordHash = PasswordHasher.HashPassword(model.Password),
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        dbContext.Users.Add(user);

        try
        {
            await dbContext.SaveChangesAsync(cancellationToken);
            await auditLogger.LogAsync(
                "Users",
                "ACCOUNT_CREATE",
                user.Id.ToString(),
                newValues: Serialize(new
                {
                    user.Username,
                    user.Email,
                    Role = model.Role,
                    AccountType = "Admin"
                }));
            return AccountProvisioningResult.Success(userId: user.Id);
        }
        catch (DbUpdateException)
        {
            return AccountProvisioningResult.Failure("Không thể tạo tài khoản vì dữ liệu bị trùng hoặc không hợp lệ.");
        }
    }

    public async Task<AccountProvisioningResult> UpdateAdminAccountAsync(
        int userId,
        AdminAccountEditViewModel model,
        CancellationToken cancellationToken = default)
    {
        var user = await dbContext.Users
            .Include(x => x.Role)
            .FirstOrDefaultAsync(x => x.Id == userId && !x.IsDeleted, cancellationToken);

        if (user is null)
        {
            return AccountProvisioningResult.Failure("Không tìm thấy tài khoản.");
        }

        if (!RoleNames.IsAdminRole(user.Role.Code))
        {
            return AccountProvisioningResult.Failure("Chỉ có thể chỉnh sửa tài khoản quản trị tại màn hình này.");
        }

        if (!RoleNames.IsAdminRole(model.Role))
        {
            return AccountProvisioningResult.Failure("Không được chuyển tài khoản quản trị sang nhóm nhân viên.");
        }

        var email = NormalizeRequired(model.Email);
        var fullName = NormalizeRequired(model.FullName);

        if (await UserExistsAsync(user.Username, email, userId, cancellationToken))
        {
            return AccountProvisioningResult.Failure("Email đã tồn tại.");
        }

        var roleId = await GetRoleIdAsync(model.Role, cancellationToken);
        if (roleId == 0)
        {
            return AccountProvisioningResult.Failure("Không tìm thấy vai trò hợp lệ.");
        }

        user.FullName = fullName;
        user.Email = email;
        user.RoleId = roleId;
        user.UpdatedAt = DateTime.UtcNow;

        try
        {
            await dbContext.SaveChangesAsync(cancellationToken);
            return AccountProvisioningResult.Success(userId: user.Id);
        }
        catch (DbUpdateException)
        {
            return AccountProvisioningResult.Failure("Không thể cập nhật tài khoản vì dữ liệu bị trùng hoặc không hợp lệ.");
        }
    }

    public async Task<AccountProvisioningResult> CreateEmployeeAccountWithProfileAsync(
        Employee employee,
        string username,
        string email,
        string password,
        bool isManager,
        Func<int, Task>? afterEmployeeCreatedAsync = null,
        CancellationToken cancellationToken = default)
    {
        var normalizedUsername = NormalizeRequired(username);
        var normalizedEmail = NormalizeRequired(email);
        var normalizedFullName = NormalizeRequired(employee.FullName);

        if (string.IsNullOrWhiteSpace(normalizedUsername) ||
            string.IsNullOrWhiteSpace(normalizedEmail) ||
            string.IsNullOrWhiteSpace(password) ||
            string.IsNullOrWhiteSpace(normalizedFullName))
        {
            return AccountProvisioningResult.Failure("Thông tin tài khoản hoặc hồ sơ chưa đầy đủ.");
        }

        if (isManager && employee.DepartmentId <= 0)
        {
            return AccountProvisioningResult.Failure("Vui lòng chọn phòng ban trước khi gán là quản lý.");
        }

        if (await UserExistsAsync(normalizedUsername, normalizedEmail, cancellationToken: cancellationToken))
        {
            return AccountProvisioningResult.Failure("Tên đăng nhập hoặc email đã tồn tại.");
        }

        var roleCode = isManager ? RoleNames.Manager : RoleNames.Employee;
        var roleId = await GetRoleIdAsync(roleCode, cancellationToken);
        if (roleId == 0)
        {
            return AccountProvisioningResult.Failure("Không tìm thấy vai trò hợp lệ.");
        }

        employee.FullName = normalizedFullName;

        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var user = new User
            {
                Username = normalizedUsername,
                FullName = normalizedFullName,
                Email = normalizedEmail,
                RoleId = roleId,
                PasswordHash = PasswordHasher.HashPassword(password),
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            employee.User = user;

            dbContext.Users.Add(user);
            dbContext.Employees.Add(employee);
            await dbContext.SaveChangesAsync(cancellationToken);

            if (afterEmployeeCreatedAsync is not null)
            {
                await afterEmployeeCreatedAsync(employee.Id);
            }

            if (isManager)
            {
                var department = await dbContext.Departments
                    .FirstOrDefaultAsync(x => x.Id == employee.DepartmentId && !x.IsDeleted, cancellationToken);

                if (department is null)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return AccountProvisioningResult.Failure("Không tìm thấy phòng ban để gán quản lý.");
                }

                department.ManagerId = user.Id;
                await dbContext.SaveChangesAsync(cancellationToken);
            }

            await auditLogger.LogAsync(
                "Users",
                "ACCOUNT_CREATE",
                user.Id.ToString(),
                newValues: Serialize(new
                {
                    user.Username,
                    user.Email,
                    Role = roleCode,
                    AccountType = "EmployeeLinked",
                    EmployeeId = employee.Id
                }));

            await transaction.CommitAsync(cancellationToken);
            return AccountProvisioningResult.Success(userId: user.Id, employeeId: employee.Id);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            return AccountProvisioningResult.Failure("Không thể tạo hồ sơ. Vui lòng thử lại.");
        }
    }

    public async Task<AccountProvisioningResult> ToggleActiveAsync(
        int userId,
        CancellationToken cancellationToken = default)
    {
        var user = await dbContext.Users
            .FirstOrDefaultAsync(x => x.Id == userId && !x.IsDeleted, cancellationToken);

        if (user is null)
        {
            return AccountProvisioningResult.Failure("Không tìm thấy tài khoản.");
        }

        user.IsActive = !user.IsActive;
        user.UpdatedAt = DateTime.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);

        await auditLogger.LogAsync(
            "Users",
            user.IsActive ? "ACCOUNT_UNLOCK" : "ACCOUNT_LOCK",
            user.Id.ToString(),
            newValues: Serialize(new { user.IsActive }));

        return AccountProvisioningResult.Success(userId: user.Id);
    }

    public async Task<AccountProvisioningResult> ResetPasswordAsync(
        int userId,
        CancellationToken cancellationToken = default)
    {
        var user = await dbContext.Users
            .FirstOrDefaultAsync(x => x.Id == userId && !x.IsDeleted, cancellationToken);

        if (user is null)
        {
            return AccountProvisioningResult.Failure("Không tìm thấy tài khoản.");
        }

        var newPassword = $"Reset@{Random.Shared.Next(100000, 999999)}";
        user.PasswordHash = PasswordHasher.HashPassword(newPassword);
        user.UpdatedAt = DateTime.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);

        await auditLogger.LogAsync(
            "Users",
            "RESET_PASSWORD",
            user.Id.ToString(),
            newValues: Serialize(new { PasswordReset = true }));

        return AccountProvisioningResult.Success(userId: user.Id, generatedPassword: newPassword);
    }

    private async Task<bool> UserExistsAsync(
        string username,
        string email,
        int? excludeUserId = null,
        CancellationToken cancellationToken = default)
    {
        var normalizedUsername = username.Trim().ToLowerInvariant();
        var normalizedEmail = email.Trim().ToLowerInvariant();

        var query = dbContext.Users.AsQueryable();
        if (excludeUserId.HasValue)
        {
            query = query.Where(x => x.Id != excludeUserId.Value);
        }

        return await query.AnyAsync(
            x => x.Username.ToLower() == normalizedUsername || x.Email.ToLower() == normalizedEmail,
            cancellationToken);
    }

    private async Task<int> GetRoleIdAsync(string roleCode, CancellationToken cancellationToken)
    {
        return await dbContext.Roles
            .Where(x => x.Code == roleCode)
            .Select(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    private static string NormalizeRequired(string? value)
    {
        return value?.Trim() ?? string.Empty;
    }

    private static string Serialize(object payload)
    {
        return JsonSerializer.Serialize(payload);
    }
}
