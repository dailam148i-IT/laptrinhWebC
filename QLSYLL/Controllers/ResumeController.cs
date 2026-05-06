using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLSYLL.Infrastructure.Auth;
using QLSYLL.Infrastructure.Extensions;
using QLSYLL.Models;

namespace QLSYLL.Controllers;

[SessionAuthorize(RoleNames.SuperAdmin, RoleNames.HrAdmin, RoleNames.Manager, RoleNames.Employee)]
public class ResumeController(ApplicationDbContext dbContext) : Controller
{
    public async Task<IActionResult> Index(string? search, int? departmentId, string? status, int page = 1, int pageSize = 5)
    {
        if (HttpContext.IsInRole(RoleNames.Employee))
        {
            return RedirectToAction(nameof(SelfEdit));
        }

        ViewData["Title"] = "Hồ sơ sơ yếu lý lịch";
        ViewData["ActiveMenu"] = "resume";
        ViewData["Breadcrumb"] = "Hồ sơ SYLL";

        var userId = HttpContext.GetCurrentUserId();
        var role = HttpContext.GetCurrentRole();
        var currentEmployee = await dbContext.Employees.FirstOrDefaultAsync(x => x.UserId == userId && !x.IsDeleted);
        var canManageResumes = HttpContext.IsInRole(RoleNames.SuperAdmin) || HttpContext.IsInRole(RoleNames.HrAdmin);

        var query = dbContext.Employees
            .Where(x => !x.IsDeleted)
            .Include(x => x.Department)
            .Include(x => x.Position)
            .Include(x => x.EmployeeEducations)
            .AsQueryable();

        if (role == RoleNames.Manager && currentEmployee is not null)
        {
            query = query.Where(x => x.DepartmentId == currentEmployee.DepartmentId);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x => x.FullName.Contains(search) || x.Phone.Contains(search));
        }

        if (departmentId.HasValue && departmentId.Value > 0)
        {
            query = query.Where(x => x.DepartmentId == departmentId.Value);
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(x => x.Status == status);
        }

        ViewBag.Search = search;
        ViewBag.SelectedDepartmentId = departmentId;
        ViewBag.SelectedStatus = status;
        ViewBag.CanManageResumes = canManageResumes;
        ViewBag.Departments = await dbContext.Departments
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.Name)
            .ToListAsync();

        var totalItems = await query.CountAsync();
        ViewBag.Page = page;
        ViewBag.PageSize = pageSize;
        ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
        ViewBag.TotalItems = totalItems;

        ViewBag.Resumes = await query
            .OrderBy(x => x.FullName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new
            {
                x.Id,
                x.FullName,
                BirthDate = x.BirthDate.ToString("dd/MM/yyyy"),
                x.Gender,
                x.Phone,
                Department = x.Department.Name,
                Position = x.Position.Name,
                x.Status,
                EducationLevel = x.EmployeeEducations.Where(e => e.IsPrimary).Select(e => e.EducationLevel).FirstOrDefault()
            })
            .ToListAsync();

        return View();
    }

    [SessionAuthorize(RoleNames.SuperAdmin, RoleNames.HrAdmin)]
    public async Task<IActionResult> Create()
    {
        await PopulateReferenceDataAsync();
        ViewData["Title"] = "Thêm hồ sơ mới";
        ViewData["ActiveMenu"] = "resume";
        ViewData["Breadcrumb"] = "Thêm mới";
        ViewData["ParentPage"] = "Hồ sơ SYLL";
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [SessionAuthorize(RoleNames.SuperAdmin, RoleNames.HrAdmin)]
    public async Task<IActionResult> Create(IFormCollection form)
    {
        var user = await dbContext.Users
            .Where(x => x.Employee == null && x.IsActive && !x.IsDeleted)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync();

        if (user is null)
        {
            TempData["ErrorMessage"] = "Không còn tài khoản trống để gán hồ sơ mới.";
            return RedirectToAction(nameof(Index));
        }

        var employee = new Employee
        {
            UserId = user.Id,
            FullName = form["FullName"]!,
            BirthDate = ParseDateOnly(form["BirthDate"]),
            Gender = form["Gender"]!,
            AliasName = NormalizeOptionalString(form["AliasName"]),
            BirthPlace = NormalizeOptionalString(form["BirthPlace"]),
            Hometown = NormalizeOptionalString(form["Hometown"]),
            PermanentAddress = NormalizeOptionalString(form["PermanentAddress"]),
            CurrentAddress = NormalizeOptionalString(form["CurrentAddress"]),
            Phone = form["Phone"]!,
            PersonalEmail = NormalizeOptionalString(form["PersonalEmail"]),
            CompanyEmail = NormalizeOptionalString(form["CompanyEmail"]),
            IdentityNumber = NormalizeOptionalString(form["IdentityNumber"]),
            IdentityIssuedDate = ParseNullableDateOnly(form["IdentityIssuedDate"]),
            IdentityIssuedPlace = NormalizeOptionalString(form["IdentityIssuedPlace"]),
            MaritalStatus = NormalizeOptionalString(form["MaritalStatus"]),
            Ethnicity = NormalizeOptionalString(form["Ethnicity"]),
            Religion = NormalizeOptionalString(form["Religion"]),
            TaxCode = NormalizeOptionalString(form["TaxCode"]),
            SocialInsuranceNumber = NormalizeOptionalString(form["SocialInsuranceNumber"]),
            BankAccountNumber = NormalizeOptionalString(form["BankAccountNumber"]),
            BankName = NormalizeOptionalString(form["BankName"]),
            BankBranch = NormalizeOptionalString(form["BankBranch"]),
            DepartmentId = ParseInt(form["DepartmentId"]),
            PositionId = ParseInt(form["PositionId"]),
            JoinDate = ParseDateOnly(form["JoinDate"]),
            YouthUnionJoinDate = ParseNullableDateOnly(form["YouthUnionJoinDate"]),
            YouthUnionJoinPlace = NormalizeOptionalString(form["YouthUnionJoinPlace"]),
            CommunistPartyJoinDate = ParseNullableDateOnly(form["CommunistPartyJoinDate"]),
            CommunistPartyJoinPlace = NormalizeOptionalString(form["CommunistPartyJoinPlace"]),
            CommunistPartyStatus = NormalizeOptionalString(form["CommunistPartyStatus"]),
            Status = "Chờ duyệt",
            Salary = 10000000,
            CreatedAt = DateTime.UtcNow
        };

        dbContext.Employees.Add(employee);
        await dbContext.SaveChangesAsync();
        await SaveEducationAndSkillsAsync(employee.Id, form);
        TempData["SuccessMessage"] = "Thêm hồ sơ mới thành công!";
        return RedirectToAction(nameof(Index));
    }

    [SessionAuthorize(RoleNames.SuperAdmin, RoleNames.HrAdmin)]
    public async Task<IActionResult> Edit(int id)
    {
        var employee = await dbContext.Employees
            .Include(x => x.Department)
            .Include(x => x.Position)
            .Include(x => x.EmployeeEducations)
            .Include(x => x.EmployeeSkills)
                .ThenInclude(x => x.Skill)
            .Include(x => x.EmployeeDocuments)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        if (employee == null) return NotFound();

        await PopulateReferenceDataAsync();
        ViewData["Title"] = "Chỉnh sửa hồ sơ";
        ViewData["ActiveMenu"] = "resume";
        ViewData["Breadcrumb"] = "Chỉnh sửa";
        ViewData["ParentPage"] = "Hồ sơ SYLL";
        ViewBag.Resume = new
        {
            employee.Id,
            employee.FullName,
            employee.AvatarPath,
            employee.Gender,
            employee.Phone,
            employee.AliasName,
            employee.BirthPlace,
            employee.Hometown,
            employee.PermanentAddress,
            employee.CurrentAddress,
            employee.PersonalEmail,
            employee.CompanyEmail,
            employee.IdentityNumber,
            IdentityIssuedDate = employee.IdentityIssuedDate?.ToString("yyyy-MM-dd"),
            employee.IdentityIssuedPlace,
            employee.MaritalStatus,
            employee.Ethnicity,
            employee.Religion,
            employee.TaxCode,
            employee.SocialInsuranceNumber,
            employee.BankAccountNumber,
            employee.BankName,
            employee.BankBranch,
            employee.DepartmentId,
            employee.PositionId,
            BirthDate = employee.BirthDate.ToString("yyyy-MM-dd"),
            JoinDate = employee.JoinDate.ToString("yyyy-MM-dd"),
            YouthUnionJoinDate = employee.YouthUnionJoinDate?.ToString("yyyy-MM-dd"),
            employee.YouthUnionJoinPlace,
            CommunistPartyJoinDate = employee.CommunistPartyJoinDate?.ToString("yyyy-MM-dd"),
            employee.CommunistPartyJoinPlace,
            employee.CommunistPartyStatus,
            Education = employee.EmployeeEducations.FirstOrDefault(e => e.IsPrimary),
            Documents = employee.EmployeeDocuments.OrderByDescending(x => x.CreatedAt).ToList()
        };
        ViewBag.SkillsText = string.Join(", ", employee.EmployeeSkills.Select(x => x.Skill.Name));
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [SessionAuthorize(RoleNames.SuperAdmin, RoleNames.HrAdmin)]
    public async Task<IActionResult> Edit(int id, IFormCollection form)
    {
        var employee = await dbContext.Employees.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        if (employee == null) return NotFound();

        employee.FullName = form["FullName"]!;
        employee.BirthDate = ParseDateOnly(form["BirthDate"], employee.BirthDate);
        employee.Gender = form["Gender"]!;
        employee.AliasName = NormalizeOptionalString(form["AliasName"]);
        employee.BirthPlace = NormalizeOptionalString(form["BirthPlace"]);
        employee.Hometown = NormalizeOptionalString(form["Hometown"]);
        employee.PermanentAddress = NormalizeOptionalString(form["PermanentAddress"]);
        employee.Phone = form["Phone"]!;
        employee.PersonalEmail = NormalizeOptionalString(form["PersonalEmail"]);
        employee.CompanyEmail = NormalizeOptionalString(form["CompanyEmail"]);
        employee.IdentityNumber = NormalizeOptionalString(form["IdentityNumber"]);
        employee.IdentityIssuedDate = ParseNullableDateOnly(form["IdentityIssuedDate"]);
        employee.IdentityIssuedPlace = NormalizeOptionalString(form["IdentityIssuedPlace"]);
        employee.CurrentAddress = NormalizeOptionalString(form["CurrentAddress"]);
        employee.MaritalStatus = NormalizeOptionalString(form["MaritalStatus"]);
        employee.Ethnicity = NormalizeOptionalString(form["Ethnicity"]);
        employee.Religion = NormalizeOptionalString(form["Religion"]);
        employee.TaxCode = NormalizeOptionalString(form["TaxCode"]);
        employee.SocialInsuranceNumber = NormalizeOptionalString(form["SocialInsuranceNumber"]);
        employee.BankAccountNumber = NormalizeOptionalString(form["BankAccountNumber"]);
        employee.BankName = NormalizeOptionalString(form["BankName"]);
        employee.BankBranch = NormalizeOptionalString(form["BankBranch"]);
        employee.DepartmentId = ParseInt(form["DepartmentId"], employee.DepartmentId);
        employee.PositionId = ParseInt(form["PositionId"], employee.PositionId);
        employee.JoinDate = ParseDateOnly(form["JoinDate"], employee.JoinDate);
        employee.YouthUnionJoinDate = ParseNullableDateOnly(form["YouthUnionJoinDate"]);
        employee.YouthUnionJoinPlace = NormalizeOptionalString(form["YouthUnionJoinPlace"]);
        employee.CommunistPartyJoinDate = ParseNullableDateOnly(form["CommunistPartyJoinDate"]);
        employee.CommunistPartyJoinPlace = NormalizeOptionalString(form["CommunistPartyJoinPlace"]);
        employee.CommunistPartyStatus = NormalizeOptionalString(form["CommunistPartyStatus"]);
        employee.UpdatedAt = DateTime.UtcNow;

        await dbContext.SaveChangesAsync();
        await SaveEducationAndSkillsAsync(employee.Id, form);
        TempData["SuccessMessage"] = "Cập nhật hồ sơ thành công!";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Details(int id)
    {
        var employee = await dbContext.Employees
            .Include(x => x.Department)
            .Include(x => x.Position)
            .Include(x => x.EmployeeEducations)
            .Include(x => x.EmployeeSkills)
                .ThenInclude(x => x.Skill)
            .Include(x => x.WorkHistories)
            .Include(x => x.FamilyMembers)
            .Include(x => x.EmployeeDocuments)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        if (employee == null) return NotFound();

        var userId = HttpContext.GetCurrentUserId();
        var role = HttpContext.GetCurrentRole();
        var currentEmployee = await dbContext.Employees.FirstOrDefaultAsync(x => x.UserId == userId && !x.IsDeleted);
        if (role == RoleNames.Manager && currentEmployee?.DepartmentId != employee.DepartmentId)
        {
            return Forbid();
        }

        ViewData["Title"] = "Chi tiết hồ sơ";
        ViewData["ActiveMenu"] = "resume";
        ViewData["Breadcrumb"] = employee.FullName;
        ViewData["ParentPage"] = "Hồ sơ SYLL";
        ViewBag.CanManageResumes = HttpContext.IsInRole(RoleNames.SuperAdmin) || HttpContext.IsInRole(RoleNames.HrAdmin);
        var primaryEducation = employee.EmployeeEducations.FirstOrDefault(e => e.IsPrimary);
        ViewBag.Resume = new
        {
            employee.Id,
            employee.FullName,
            employee.AvatarPath,
            BirthDate = employee.BirthDate.ToString("dd/MM/yyyy"),
            employee.Gender,
            employee.AliasName,
            employee.BirthPlace,
            employee.Hometown,
            employee.PermanentAddress,
            employee.CurrentAddress,
            employee.Phone,
            employee.PersonalEmail,
            employee.CompanyEmail,
            employee.IdentityNumber,
            IdentityIssuedDate = employee.IdentityIssuedDate?.ToString("dd/MM/yyyy"),
            employee.IdentityIssuedPlace,
            employee.MaritalStatus,
            employee.Ethnicity,
            employee.Religion,
            employee.TaxCode,
            employee.SocialInsuranceNumber,
            employee.BankAccountNumber,
            employee.BankName,
            employee.BankBranch,
            Department = employee.Department.Name,
            Position = employee.Position.Name,
            employee.Status,
            JoinDate = employee.JoinDate.ToString("dd/MM/yyyy"),
            EducationLevel = primaryEducation?.EducationLevel,
            GeneralEducationLevel = primaryEducation?.GeneralEducationLevel,
            SchoolName = primaryEducation?.SchoolName,
            Major = primaryEducation?.Major,
            GraduationYear = primaryEducation?.GraduationYear,
            Ranking = primaryEducation?.Ranking,
            ForeignLanguage = primaryEducation?.ForeignLanguage,
            ITLevel = primaryEducation?.ITLevel,
            Skills = string.Join(", ", employee.EmployeeSkills.Select(s => s.Skill.Name)),
            YouthUnionJoinDate = employee.YouthUnionJoinDate?.ToString("dd/MM/yyyy"),
            employee.YouthUnionJoinPlace,
            CommunistPartyJoinDate = employee.CommunistPartyJoinDate?.ToString("dd/MM/yyyy"),
            employee.CommunistPartyJoinPlace,
            employee.CommunistPartyStatus,
            WorkHistories = employee.WorkHistories.OrderByDescending(w => w.StartDate).ToList(),
            Documents = employee.EmployeeDocuments.OrderByDescending(d => d.CreatedAt).ToList(),
            FamilyMembers = employee.FamilyMembers
                .OrderByDescending(f => f.IsEmergencyContact)
                .ThenBy(f => f.Relationship)
                .ToList()
        };
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [SessionAuthorize(RoleNames.SuperAdmin, RoleNames.HrAdmin)]
    public async Task<IActionResult> Delete(int id)
    {
        var employee = await dbContext.Employees.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        if (employee == null) return NotFound();

        if (HttpContext.IsInRole(RoleNames.HrAdmin))
        {
            employee.IsDeleted = true;
            employee.DeletedAt = DateTime.UtcNow;
            employee.DeletedBy = HttpContext.GetCurrentUserId();
        }
        else
        {
            dbContext.Employees.Remove(employee);
        }

        await dbContext.SaveChangesAsync();
        TempData["SuccessMessage"] = "Xóa hồ sơ thành công!";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> SelfEdit()
    {
        var userId = HttpContext.GetCurrentUserId();
        if (!userId.HasValue)
        {
            return RedirectToAction("Login", "Account");
        }

        var employee = await dbContext.Employees
            .Include(x => x.Department)
            .Include(x => x.Position)
            .Include(x => x.EmployeeEducations)
            .Include(x => x.EmployeeSkills)
                .ThenInclude(x => x.Skill)
            .Include(x => x.WorkHistories)
            .Include(x => x.EmployeeDocuments)
            .FirstOrDefaultAsync(x => x.UserId == userId && !x.IsDeleted);
        if (employee == null) return NotFound();

        ViewData["Title"] = "Hồ sơ cá nhân";
        ViewData["ActiveMenu"] = "profile";
        ViewData["Breadcrumb"] = "Hồ sơ cá nhân";
        ViewBag.Resume = employee;
        ViewBag.PrimaryEducation = employee.EmployeeEducations.FirstOrDefault(e => e.IsPrimary);
        ViewBag.SkillsText = string.Join(", ", employee.EmployeeSkills.Select(x => x.Skill.Name));
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SelfEdit(IFormCollection form)
    {
        var userId = HttpContext.GetCurrentUserId();
        if (!userId.HasValue)
        {
            return RedirectToAction("Login", "Account");
        }

        var employee = await dbContext.Employees.Include(x => x.WorkHistories).FirstOrDefaultAsync(x => x.UserId == userId && !x.IsDeleted);
        if (employee == null) return NotFound();

        employee.Phone = form["Phone"]!;
        employee.PersonalEmail = form["PersonalEmail"];
        employee.CurrentAddress = form["CurrentAddress"];
        employee.UpdatedAt = DateTime.UtcNow;

        await dbContext.SaveChangesAsync();
        await SaveEducationAndSkillsAsync(employee.Id, form);
        TempData["SuccessMessage"] = "Cập nhật hồ sơ cá nhân thành công!";
        return RedirectToAction(nameof(SelfEdit));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UploadAvatar(IFormFile? avatarFile)
    {
        var userId = HttpContext.GetCurrentUserId();
        if (!userId.HasValue || avatarFile is null || avatarFile.Length == 0)
        {
            TempData["ErrorMessage"] = "Tệp ảnh không hợp lệ.";
            return RedirectToAction(nameof(SelfEdit));
        }

        var employee = await dbContext.Employees.FirstOrDefaultAsync(x => x.UserId == userId.Value && !x.IsDeleted);
        if (employee is null)
        {
            return RedirectToAction(nameof(SelfEdit));
        }

        var extension = Path.GetExtension(avatarFile.FileName).ToLowerInvariant();
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
        if (!allowedExtensions.Contains(extension) || avatarFile.Length > 2 * 1024 * 1024)
        {
            TempData["ErrorMessage"] = "Chỉ chấp nhận JPG/PNG và tối đa 2MB.";
            return RedirectToAction(nameof(SelfEdit));
        }

        employee.AvatarPath = await SaveFileAsync(avatarFile, "avatars", employee.Id.ToString());
        employee.UpdatedAt = DateTime.UtcNow;
        await dbContext.SaveChangesAsync();

        TempData["SuccessMessage"] = "Cập nhật ảnh đại diện thành công!";
        return RedirectToAction(nameof(SelfEdit));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [SessionAuthorize(RoleNames.SuperAdmin, RoleNames.HrAdmin)]
    public async Task<IActionResult> UploadAvatarForEmployee(int id, IFormFile? avatarFile)
    {
        var employee = await dbContext.Employees.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        if (employee is null || avatarFile is null || avatarFile.Length == 0)
        {
            TempData["ErrorMessage"] = "Tệp ảnh không hợp lệ.";
            return RedirectToAction(nameof(Edit), new { id });
        }

        var extension = Path.GetExtension(avatarFile.FileName).ToLowerInvariant();
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
        if (!allowedExtensions.Contains(extension) || avatarFile.Length > 2 * 1024 * 1024)
        {
            TempData["ErrorMessage"] = "Chỉ chấp nhận JPG/PNG và tối đa 2MB.";
            return RedirectToAction(nameof(Edit), new { id });
        }

        employee.AvatarPath = await SaveFileAsync(avatarFile, "avatars", employee.Id.ToString());
        employee.UpdatedAt = DateTime.UtcNow;
        await dbContext.SaveChangesAsync();

        TempData["SuccessMessage"] = "Đã cập nhật ảnh nhân viên.";
        return RedirectToAction(nameof(Edit), new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UploadDocument(IFormFile? documentFile, string? title, string? category, int? employeeId, string? returnAction)
    {
        if (documentFile is null || documentFile.Length == 0)
        {
            TempData["ErrorMessage"] = "Tệp tài liệu không hợp lệ.";
            return RedirectToRequestedPage(returnAction, employeeId);
        }

        var employee = await ResolveEmployeeForDocumentAsync(employeeId);
        if (employee is null)
        {
            return Forbid();
        }

        var extension = Path.GetExtension(documentFile.FileName).ToLowerInvariant();
        var allowedExtensions = new[] { ".pdf", ".doc", ".docx", ".jpg", ".jpeg", ".png" };
        if (!allowedExtensions.Contains(extension) || documentFile.Length > 10 * 1024 * 1024)
        {
            TempData["ErrorMessage"] = "Chỉ chấp nhận PDF, Word, JPG, PNG và tối đa 10MB.";
            return RedirectToRequestedPage(returnAction, employee.Id);
        }

        var storedPath = await SaveFileAsync(documentFile, "documents", employee.Id.ToString());
        dbContext.EmployeeDocuments.Add(new EmployeeDocument
        {
            EmployeeId = employee.Id,
            Title = NormalizeOptionalString(title) ?? Path.GetFileNameWithoutExtension(documentFile.FileName),
            Category = NormalizeOptionalString(category) ?? "Khac",
            FileName = documentFile.FileName,
            FilePath = storedPath,
            ContentType = documentFile.ContentType,
            FileSize = documentFile.Length,
            UploadedByUserId = HttpContext.GetCurrentUserId(),
            CreatedAt = DateTime.UtcNow
        });

        await dbContext.SaveChangesAsync();
        TempData["SuccessMessage"] = "Đã tải tài liệu lên thành công.";
        return RedirectToRequestedPage(returnAction, employee.Id);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteDocument(int id, string? returnAction)
    {
        var document = await dbContext.EmployeeDocuments
            .Include(x => x.Employee)
            .FirstOrDefaultAsync(x => x.Id == id && !x.Employee.IsDeleted);
        if (document is null)
        {
            return NotFound();
        }

        var currentUserId = HttpContext.GetCurrentUserId();
        var canManageAll = HttpContext.IsInRole(RoleNames.SuperAdmin) || HttpContext.IsInRole(RoleNames.HrAdmin);
        if (!canManageAll && (!currentUserId.HasValue || document.Employee.UserId != currentUserId.Value))
        {
            return Forbid();
        }

        DeletePhysicalFile(document.FilePath);
        dbContext.EmployeeDocuments.Remove(document);
        await dbContext.SaveChangesAsync();

        TempData["SuccessMessage"] = "Đã xóa tài liệu.";
        return RedirectToRequestedPage(returnAction, document.EmployeeId);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddWorkHistory(IFormCollection form)
    {
        var userId = HttpContext.GetCurrentUserId();
        if (!userId.HasValue)
        {
            return RedirectToAction("Login", "Account");
        }

        var employee = await dbContext.Employees.FirstOrDefaultAsync(x => x.UserId == userId.Value && !x.IsDeleted);
        if (employee is null)
        {
            return RedirectToAction(nameof(SelfEdit));
        }

        dbContext.WorkHistories.Add(new WorkHistory
        {
            EmployeeId = employee.Id,
            CompanyName = form["CompanyName"]!,
            PositionTitle = form["PositionTitle"]!,
            StartDate = ParseDateOnly(form["StartDate"]),
            EndDate = string.IsNullOrWhiteSpace(form["EndDate"]) ? null : ParseDateOnly(form["EndDate"]),
            Description = form["Description"]
        });

        await dbContext.SaveChangesAsync();
        TempData["SuccessMessage"] = "Đã thêm quá trình công tác.";
        return RedirectToAction(nameof(SelfEdit));
    }

    [HttpGet]
    public async Task<IActionResult> SearchBySkill(string term)
    {
        var userId = HttpContext.GetCurrentUserId();
        var role = HttpContext.GetCurrentRole();
        var currentEmployee = await dbContext.Employees.FirstOrDefaultAsync(x => x.UserId == userId && !x.IsDeleted);

        var query = dbContext.EmployeeSkills
            .Where(x => x.Skill.Name.Contains(term))
            .AsQueryable();

        if (role == RoleNames.Manager && currentEmployee is not null)
        {
            query = query.Where(x => x.Employee.DepartmentId == currentEmployee.DepartmentId);
        }

        var data = await query
            .Select(x => new
            {
                x.EmployeeId,
                x.Employee.FullName,
                Skill = x.Skill.Name
            })
            .Distinct()
            .Take(20)
            .ToListAsync();

        return Json(data);
    }

    private async Task PopulateReferenceDataAsync()
    {
        ViewBag.Departments = await dbContext.Departments.Where(x => !x.IsDeleted).OrderBy(x => x.Name).ToListAsync();
        ViewBag.Positions = await dbContext.Positions.Where(x => !x.IsDeleted).OrderBy(x => x.Level).ToListAsync();
    }

    private async Task<Employee?> ResolveEmployeeForDocumentAsync(int? employeeId)
    {
        if (HttpContext.IsInRole(RoleNames.SuperAdmin) || HttpContext.IsInRole(RoleNames.HrAdmin))
        {
            if (!employeeId.HasValue)
            {
                return null;
            }

            return await dbContext.Employees.FirstOrDefaultAsync(x => x.Id == employeeId.Value && !x.IsDeleted);
        }

        var userId = HttpContext.GetCurrentUserId();
        if (!userId.HasValue)
        {
            return null;
        }

        return await dbContext.Employees.FirstOrDefaultAsync(x => x.UserId == userId.Value && !x.IsDeleted);
    }

    private IActionResult RedirectToRequestedPage(string? returnAction, int? employeeId)
    {
        if (string.Equals(returnAction, nameof(Edit), StringComparison.OrdinalIgnoreCase) && employeeId.HasValue)
        {
            return RedirectToAction(nameof(Edit), new { id = employeeId.Value });
        }

        if (string.Equals(returnAction, nameof(Details), StringComparison.OrdinalIgnoreCase) && employeeId.HasValue)
        {
            return RedirectToAction(nameof(Details), new { id = employeeId.Value });
        }

        return RedirectToAction(nameof(SelfEdit));
    }

    private static async Task<string> SaveFileAsync(IFormFile file, string folderName, string filePrefix)
    {
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", folderName);
        Directory.CreateDirectory(uploadFolder);

        var safePrefix = string.Concat(filePrefix.Where(char.IsLetterOrDigit));
        var fileName = $"{safePrefix}_{DateTime.UtcNow:yyyyMMddHHmmssfff}{extension}";
        var filePath = Path.Combine(uploadFolder, fileName);

        await using var stream = System.IO.File.Create(filePath);
        await file.CopyToAsync(stream);

        return $"/uploads/{folderName}/{fileName}";
    }

    private static void DeletePhysicalFile(string? relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath))
        {
            return;
        }

        var trimmedPath = relativePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", trimmedPath);
        if (System.IO.File.Exists(fullPath))
        {
            System.IO.File.Delete(fullPath);
        }
    }

    private async Task SaveEducationAndSkillsAsync(int employeeId, IFormCollection form)
    {
        var education = await dbContext.EmployeeEducations.FirstOrDefaultAsync(x => x.EmployeeId == employeeId && x.IsPrimary);
        if (education == null)
        {
            education = new EmployeeEducation { EmployeeId = employeeId, IsPrimary = true };
            dbContext.EmployeeEducations.Add(education);
        }

        education.EducationLevel = form["EducationLevel"]!;
        education.GeneralEducationLevel = NormalizeOptionalString(form["GeneralEducationLevel"]);
        education.SchoolName = NormalizeOptionalString(form["SchoolName"]);
        education.Major = NormalizeOptionalString(form["Major"]);
        education.GraduationYear = ParseNullableInt(form["GraduationYear"]);
        education.Ranking = NormalizeOptionalString(form["Ranking"]);
        education.ForeignLanguage = NormalizeOptionalString(form["ForeignLanguage"]);
        education.ITLevel = NormalizeOptionalString(form["ITLevel"]);

        var skillNames = form["Skills"].ToString()
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        var existingLinks = await dbContext.EmployeeSkills.Where(x => x.EmployeeId == employeeId).ToListAsync();
        dbContext.EmployeeSkills.RemoveRange(existingLinks);
        await dbContext.SaveChangesAsync();

        foreach (var skillName in skillNames)
        {
            var skill = await dbContext.Skills.FirstOrDefaultAsync(x => x.Name == skillName);
            if (skill == null)
            {
                skill = new Skill { Name = skillName };
                dbContext.Skills.Add(skill);
                await dbContext.SaveChangesAsync();
            }

            dbContext.EmployeeSkills.Add(new EmployeeSkill { EmployeeId = employeeId, SkillId = skill.Id });
        }

        await dbContext.SaveChangesAsync();
    }

    private static DateOnly ParseDateOnly(string? raw, DateOnly? fallback = null)
    {
        return DateOnly.TryParse(raw, out var date) ? date : fallback ?? DateOnly.FromDateTime(DateTime.Today);
    }

    private static DateOnly? ParseNullableDateOnly(string? raw)
    {
        return DateOnly.TryParse(raw, out var date) ? date : null;
    }

    private static int ParseInt(string? raw, int fallback = 0)
    {
        return int.TryParse(raw, out var value) ? value : fallback;
    }

    private static int? ParseNullableInt(string? raw)
    {
        return int.TryParse(raw, out var value) ? value : null;
    }

    private static string? NormalizeOptionalString(string? value)
    {
        var normalized = value?.Trim();
        return string.IsNullOrWhiteSpace(normalized) ? null : normalized;
    }
}
