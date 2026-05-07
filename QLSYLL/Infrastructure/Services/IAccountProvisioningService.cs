using QLSYLL.Models;
using QLSYLL.Models.ViewModels;

namespace QLSYLL.Infrastructure.Services;

public interface IAccountProvisioningService
{
    Task<AccountProvisioningResult> CreateAdminAccountAsync(
        AdminAccountCreateViewModel model,
        CancellationToken cancellationToken = default);

    Task<AccountProvisioningResult> UpdateAdminAccountAsync(
        int userId,
        AdminAccountEditViewModel model,
        CancellationToken cancellationToken = default);

    Task<AccountProvisioningResult> CreateEmployeeAccountWithProfileAsync(
        Employee employee,
        string username,
        string email,
        string password,
        bool isManager,
        Func<int, Task>? afterEmployeeCreatedAsync = null,
        CancellationToken cancellationToken = default);

    Task<AccountProvisioningResult> ToggleActiveAsync(
        int userId,
        CancellationToken cancellationToken = default);

    Task<AccountProvisioningResult> ResetPasswordAsync(
        int userId,
        CancellationToken cancellationToken = default);
}
