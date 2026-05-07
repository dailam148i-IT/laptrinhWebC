namespace QLSYLL.Infrastructure.Services;

public sealed class AccountProvisioningResult
{
    private AccountProvisioningResult(bool succeeded, string? errorMessage, int? userId, int? employeeId, string? generatedPassword)
    {
        Succeeded = succeeded;
        ErrorMessage = errorMessage;
        UserId = userId;
        EmployeeId = employeeId;
        GeneratedPassword = generatedPassword;
    }

    public bool Succeeded { get; }
    public string? ErrorMessage { get; }
    public int? UserId { get; }
    public int? EmployeeId { get; }
    public string? GeneratedPassword { get; }

    public static AccountProvisioningResult Success(int? userId = null, int? employeeId = null, string? generatedPassword = null)
    {
        return new AccountProvisioningResult(true, null, userId, employeeId, generatedPassword);
    }

    public static AccountProvisioningResult Failure(string errorMessage)
    {
        return new AccountProvisioningResult(false, errorMessage, null, null, null);
    }
}
