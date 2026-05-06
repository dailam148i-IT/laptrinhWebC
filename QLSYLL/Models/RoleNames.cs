namespace QLSYLL.Models;

public static class RoleNames
{
    public const string SuperAdmin = "SA";
    public const string HrAdmin = "HR";
    public const string Manager = "Manager";
    public const string Employee = "Employee";

    public static readonly string[] All = [SuperAdmin, HrAdmin, Manager, Employee];
}
