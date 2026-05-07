namespace QLSYLL.Models;

public static class RoleNames
{
    public const string SuperAdmin = "SA";
    public const string HrAdmin = "HR";
    public const string Manager = "Manager";
    public const string Employee = "Employee";

    public static readonly string[] All = [SuperAdmin, HrAdmin, Manager, Employee];
    public static readonly string[] AdminRoles = [SuperAdmin, HrAdmin];
    public static readonly string[] EmployeeLinkedRoles = [Manager, Employee];

    public static bool IsAdminRole(string? roleCode)
    {
        return roleCode is SuperAdmin or HrAdmin;
    }

    public static bool IsEmployeeLinkedRole(string? roleCode)
    {
        return roleCode is Manager or Employee;
    }
}
