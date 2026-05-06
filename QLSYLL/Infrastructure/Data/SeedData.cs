using QLSYLL.Infrastructure.Security;
using QLSYLL.Models;

namespace QLSYLL.Infrastructure.Data;

public static class SeedData
{
    public static readonly DateTime SeedCreatedAt = new(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public static IEnumerable<Role> Roles =>
    [
        new() { Id = 1, Code = RoleNames.SuperAdmin, Name = "Super Admin" },
        new() { Id = 2, Code = RoleNames.HrAdmin, Name = "Nhân sự" },
        new() { Id = 3, Code = RoleNames.Manager, Name = "Trưởng phòng" },
        new() { Id = 4, Code = RoleNames.Employee, Name = "Nhân viên" }
    ];

    public static IEnumerable<User> Users =>
    [
        new() { Id = 1, Username = "superadmin", PasswordHash = PasswordHasher.HashPassword("Admin@123"), FullName = "Đặng Quốc V", Email = "superadmin@qlsyll.vn", RoleId = 1, CreatedAt = SeedCreatedAt, IsActive = true },
        new() { Id = 2, Username = "hradmin", PasswordHash = PasswordHasher.HashPassword("Hr@12345"), FullName = "Bùi Thị H", Email = "hradmin@qlsyll.vn", RoleId = 2, CreatedAt = SeedCreatedAt.AddDays(1), IsActive = true },
        new() { Id = 3, Username = "manager01", PasswordHash = PasswordHasher.HashPassword("Manager@123"), FullName = "Vũ Văn G", Email = "manager01@qlsyll.vn", RoleId = 3, CreatedAt = SeedCreatedAt.AddDays(2), IsActive = true },
        new() { Id = 4, Username = "nhanvien01", PasswordHash = PasswordHasher.HashPassword("Nv@123456"), FullName = "Nguyễn Văn A", Email = "nva@qlsyll.vn", RoleId = 4, CreatedAt = SeedCreatedAt.AddDays(3), IsActive = true },
        new() { Id = 5, Username = "nhanvien02", PasswordHash = PasswordHasher.HashPassword("Nv@123456"), FullName = "Trần Thị B", Email = "ttb@qlsyll.vn", RoleId = 4, CreatedAt = SeedCreatedAt.AddDays(4), IsActive = true },
        new() { Id = 6, Username = "nhanvien03", PasswordHash = PasswordHasher.HashPassword("Nv@123456"), FullName = "Lê Văn C", Email = "lvc@qlsyll.vn", RoleId = 4, CreatedAt = SeedCreatedAt.AddDays(5), IsActive = false },
        new() { Id = 7, Username = "manager02", PasswordHash = PasswordHasher.HashPassword("Manager@123"), FullName = "Trần Văn K", Email = "manager02@qlsyll.vn", RoleId = 3, CreatedAt = SeedCreatedAt.AddDays(6), IsActive = true },
        new() { Id = 8, Username = "nhanvien04", PasswordHash = PasswordHasher.HashPassword("Nv@123456"), FullName = "Phạm Thị D", Email = "ptd@qlsyll.vn", RoleId = 4, CreatedAt = SeedCreatedAt.AddDays(7), IsActive = true }
    ];

    public static IEnumerable<Department> Departments =>
    [
        new() { Id = 1, Name = "Phòng Kỹ thuật", Code = "KT", Description = "Phát triển hệ thống", ManagerId = 3, CreatedAt = SeedCreatedAt },
        new() { Id = 2, Name = "Phòng Nhân sự", Code = "NS", Description = "Quản trị nhân sự", ManagerId = 2, CreatedAt = SeedCreatedAt },
        new() { Id = 3, Name = "Phòng Kinh doanh", Code = "KD", Description = "Kinh doanh và bán hàng", ManagerId = 7, CreatedAt = SeedCreatedAt.AddDays(5) },
        new() { Id = 4, Name = "Phòng Tài chính", Code = "TC", Description = "Tài chính kế toán", ManagerId = null, CreatedAt = SeedCreatedAt.AddDays(10) }
    ];

    public static IEnumerable<Position> Positions =>
    [
        new() { Id = 1, Name = "Giám đốc", Level = 1, BaseSalary = 50000000, Description = "Điều hành công ty" },
        new() { Id = 2, Name = "Trưởng phòng", Level = 3, BaseSalary = 25000000, Description = "Quản lý phòng ban" },
        new() { Id = 3, Name = "Chuyên viên", Level = 5, BaseSalary = 15000000, Description = "Chuyên viên nghiệp vụ" },
        new() { Id = 4, Name = "Nhân viên", Level = 6, BaseSalary = 10000000, Description = "Nhân viên chính thức" }
    ];

    public static IEnumerable<Employee> Employees =>
    [
        new() { Id = 1, UserId = 1, FullName = "ĐẶNG QUỐC V", BirthDate = new DateOnly(1982, 2, 10), Gender = "Nam", AliasName = "Không", BirthPlace = "TP.HCM", Hometown = "Quận 1, TP.HCM", PermanentAddress = "Quận 1, TP.HCM", CurrentAddress = "Quận 1, TP.HCM", Phone = "0900000001", PersonalEmail = "superadmin@qlsyll.vn", CompanyEmail = "v.dang@company.vn", IdentityNumber = "079082000001", IdentityIssuedDate = new DateOnly(2021, 1, 5), IdentityIssuedPlace = "Cục CSQLHC", MaritalStatus = "Đã kết hôn", Ethnicity = "Kinh", Religion = "Không", TaxCode = "0311111111", SocialInsuranceNumber = "BHXH001", BankAccountNumber = "123456789", BankName = "Vietcombank", BankBranch = "Tân Định", DepartmentId = 4, PositionId = 1, JoinDate = new DateOnly(2020, 1, 1), Status = "Đã duyệt", Salary = 60000000, FamilyInfo = "Đã kết hôn", YouthUnionJoinDate = new DateOnly(1998, 3, 26), YouthUnionJoinPlace = "TP.HCM", CommunistPartyJoinDate = new DateOnly(2008, 5, 19), CommunistPartyJoinPlace = "TP.HCM", CommunistPartyStatus = "Chính thức", CreatedAt = SeedCreatedAt },
        new() { Id = 2, UserId = 2, FullName = "BÙI THỊ H", BirthDate = new DateOnly(1989, 12, 14), Gender = "Nữ", AliasName = "Không", BirthPlace = "Nam Định", Hometown = "Nam Định", PermanentAddress = "Quận 3, TP.HCM", CurrentAddress = "Quận 3, TP.HCM", Phone = "0900000002", PersonalEmail = "hradmin@qlsyll.vn", CompanyEmail = "h.bui@company.vn", IdentityNumber = "079082000002", IdentityIssuedDate = new DateOnly(2020, 6, 10), IdentityIssuedPlace = "Cục CSQLHC", MaritalStatus = "Đã kết hôn", Ethnicity = "Kinh", Religion = "Không", TaxCode = "0311111112", SocialInsuranceNumber = "BHXH002", BankAccountNumber = "223456789", BankName = "ACB", BankBranch = "Quận 3", DepartmentId = 2, PositionId = 2, JoinDate = new DateOnly(2021, 2, 1), Status = "Đã duyệt", Salary = 28000000, FamilyInfo = "1 con", YouthUnionJoinDate = new DateOnly(2004, 3, 26), YouthUnionJoinPlace = "Nam Định", CreatedAt = SeedCreatedAt.AddDays(1) },
        new() { Id = 3, UserId = 3, FullName = "VŨ VĂN G", BirthDate = new DateOnly(1987, 6, 27), Gender = "Nam", BirthPlace = "Hà Nội", Hometown = "Hà Nội", PermanentAddress = "Thủ Đức, TP.HCM", CurrentAddress = "Thủ Đức, TP.HCM", Phone = "0900000003", PersonalEmail = "manager01@qlsyll.vn", CompanyEmail = "g.vu@company.vn", IdentityNumber = "079082000003", IdentityIssuedDate = new DateOnly(2019, 4, 12), IdentityIssuedPlace = "Cục CSQLHC", MaritalStatus = "Độc thân", Ethnicity = "Kinh", Religion = "Không", TaxCode = "0311111113", SocialInsuranceNumber = "BHXH003", BankAccountNumber = "323456789", BankName = "Techcombank", BankBranch = "Thủ Đức", DepartmentId = 1, PositionId = 2, JoinDate = new DateOnly(2022, 1, 10), Status = "Đã duyệt", Salary = 26000000, FamilyInfo = "Độc thân", CreatedAt = SeedCreatedAt.AddDays(2) },
        new() { Id = 4, UserId = 4, FullName = "NGUYỄN VĂN A", BirthDate = new DateOnly(1990, 3, 15), Gender = "Nam", BirthPlace = "Bình Định", Hometown = "Bình Định", PermanentAddress = "Bình Thạnh, TP.HCM", CurrentAddress = "Bình Thạnh, TP.HCM", Phone = "0901234567", PersonalEmail = "nva@qlsyll.vn", CompanyEmail = "a.nguyen@company.vn", IdentityNumber = "079082000004", IdentityIssuedDate = new DateOnly(2022, 2, 15), IdentityIssuedPlace = "Cục CSQLHC", MaritalStatus = "Độc thân", Ethnicity = "Kinh", Religion = "Không", TaxCode = "0311111114", SocialInsuranceNumber = "BHXH004", BankAccountNumber = "423456789", BankName = "MB Bank", BankBranch = "Bình Thạnh", DepartmentId = 1, PositionId = 4, JoinDate = new DateOnly(2024, 3, 1), Status = "Đã duyệt", Salary = 12000000, FamilyInfo = "Độc thân", CreatedAt = SeedCreatedAt.AddDays(3) },
        new() { Id = 5, UserId = 5, FullName = "TRẦN THỊ B", BirthDate = new DateOnly(1992, 7, 22), Gender = "Nữ", BirthPlace = "Huế", Hometown = "Huế", PermanentAddress = "Gò Vấp, TP.HCM", CurrentAddress = "Gò Vấp, TP.HCM", Phone = "0912345678", PersonalEmail = "ttb@qlsyll.vn", CompanyEmail = "b.tran@company.vn", IdentityNumber = "079082000005", IdentityIssuedDate = new DateOnly(2021, 8, 20), IdentityIssuedPlace = "Cục CSQLHC", MaritalStatus = "Đã kết hôn", Ethnicity = "Kinh", Religion = "Không", TaxCode = "0311111115", SocialInsuranceNumber = "BHXH005", BankAccountNumber = "523456789", BankName = "Vietinbank", BankBranch = "Gò Vấp", DepartmentId = 2, PositionId = 3, JoinDate = new DateOnly(2024, 1, 15), Status = "Đã duyệt", Salary = 16000000, FamilyInfo = "Đã kết hôn", CreatedAt = SeedCreatedAt.AddDays(4) },
        new() { Id = 6, UserId = 6, FullName = "LÊ VĂN C", BirthDate = new DateOnly(1988, 11, 10), Gender = "Nam", BirthPlace = "Đồng Nai", Hometown = "Đồng Nai", PermanentAddress = "Quận 7, TP.HCM", CurrentAddress = "Quận 7, TP.HCM", Phone = "0923456789", PersonalEmail = "lvc@qlsyll.vn", CompanyEmail = "c.le@company.vn", IdentityNumber = "079082000006", IdentityIssuedDate = new DateOnly(2018, 7, 3), IdentityIssuedPlace = "Cục CSQLHC", MaritalStatus = "Đã kết hôn", Ethnicity = "Kinh", Religion = "Không", TaxCode = "0311111116", SocialInsuranceNumber = "BHXH006", BankAccountNumber = "623456789", BankName = "Sacombank", BankBranch = "Quận 7", DepartmentId = 3, PositionId = 4, JoinDate = new DateOnly(2024, 2, 20), Status = "Chờ duyệt", Salary = 11000000, FamilyInfo = "1 con", CreatedAt = SeedCreatedAt.AddDays(5) },
        new() { Id = 7, UserId = 7, FullName = "TRẦN VĂN K", BirthDate = new DateOnly(1985, 9, 9), Gender = "Nam", BirthPlace = "Long An", Hometown = "Long An", PermanentAddress = "Tân Bình, TP.HCM", CurrentAddress = "Tân Bình, TP.HCM", Phone = "0934567890", PersonalEmail = "manager02@qlsyll.vn", CompanyEmail = "k.tran@company.vn", IdentityNumber = "079082000007", IdentityIssuedDate = new DateOnly(2017, 9, 10), IdentityIssuedPlace = "Cục CSQLHC", MaritalStatus = "Đã kết hôn", Ethnicity = "Kinh", Religion = "Không", TaxCode = "0311111117", SocialInsuranceNumber = "BHXH007", BankAccountNumber = "723456789", BankName = "BIDV", BankBranch = "Tân Bình", DepartmentId = 3, PositionId = 2, JoinDate = new DateOnly(2023, 6, 1), Status = "Đã duyệt", Salary = 25500000, FamilyInfo = "Đã kết hôn", CreatedAt = SeedCreatedAt.AddDays(6) },
        new() { Id = 8, UserId = 8, FullName = "PHẠM THỊ D", BirthDate = new DateOnly(1995, 5, 5), Gender = "Nữ", BirthPlace = "Cần Thơ", Hometown = "Cần Thơ", PermanentAddress = "Phú Nhuận, TP.HCM", CurrentAddress = "Phú Nhuận, TP.HCM", Phone = "0945678901", PersonalEmail = "ptd@qlsyll.vn", CompanyEmail = "d.pham@company.vn", IdentityNumber = "079082000008", IdentityIssuedDate = new DateOnly(2020, 12, 25), IdentityIssuedPlace = "Cục CSQLHC", MaritalStatus = "Độc thân", Ethnicity = "Kinh", Religion = "Không", TaxCode = "0311111118", SocialInsuranceNumber = "BHXH008", BankAccountNumber = "823456789", BankName = "TPBank", BankBranch = "Phú Nhuận", DepartmentId = 4, PositionId = 3, JoinDate = new DateOnly(2024, 3, 5), Status = "Bị khóa", Salary = 17000000, FamilyInfo = "Độc thân", CreatedAt = SeedCreatedAt.AddDays(7) }
    ];

    public static IEnumerable<Skill> Skills =>
    [
        new() { Id = 1, Name = "C#" },
        new() { Id = 2, Name = "ASP.NET MVC" },
        new() { Id = 3, Name = "SQL Server" },
        new() { Id = 4, Name = "Quản trị nhân sự" },
        new() { Id = 5, Name = "Tuyển dụng" },
        new() { Id = 6, Name = "Kế toán" },
        new() { Id = 7, Name = "Bán hàng" },
        new() { Id = 8, Name = "Quản lý dự án" }
    ];

    public static IEnumerable<EmployeeSkill> EmployeeSkills =>
    [
        new() { EmployeeId = 1, SkillId = 8 },
        new() { EmployeeId = 2, SkillId = 4 },
        new() { EmployeeId = 2, SkillId = 5 },
        new() { EmployeeId = 3, SkillId = 8 },
        new() { EmployeeId = 4, SkillId = 1 },
        new() { EmployeeId = 4, SkillId = 2 },
        new() { EmployeeId = 4, SkillId = 3 },
        new() { EmployeeId = 5, SkillId = 4 },
        new() { EmployeeId = 5, SkillId = 5 },
        new() { EmployeeId = 6, SkillId = 7 },
        new() { EmployeeId = 7, SkillId = 7 },
        new() { EmployeeId = 8, SkillId = 6 }
    ];

    public static IEnumerable<EmployeeEducation> EmployeeEducations =>
    [
        new() { Id = 1, EmployeeId = 1, EducationLevel = "Sau đại học", GeneralEducationLevel = "12/12", SchoolName = "ĐH Kinh tế", Major = "Quản trị", GraduationYear = 2005, Ranking = "Giỏi", ForeignLanguage = "Tiếng Anh B2", ITLevel = "MOS", IsPrimary = true },
        new() { Id = 2, EmployeeId = 2, EducationLevel = "Đại học", GeneralEducationLevel = "12/12", SchoolName = "ĐH Lao động", Major = "Quản trị nhân sự", GraduationYear = 2012, Ranking = "Khá", ForeignLanguage = "Tiếng Anh TOEIC 650", ITLevel = "Tin học văn phòng", IsPrimary = true },
        new() { Id = 3, EmployeeId = 3, EducationLevel = "Đại học", GeneralEducationLevel = "12/12", SchoolName = "ĐH Bách khoa", Major = "CNTT", GraduationYear = 2010, Ranking = "Giỏi", ForeignLanguage = "Tiếng Anh TOEIC 750", ITLevel = "Nâng cao", IsPrimary = true },
        new() { Id = 4, EmployeeId = 4, EducationLevel = "Đại học", GeneralEducationLevel = "12/12", SchoolName = "ĐH Bách khoa", Major = "Công nghệ thông tin", GraduationYear = 2020, Ranking = "Giỏi", ForeignLanguage = "IELTS 6.5", ITLevel = "Lập trình", IsPrimary = true },
        new() { Id = 5, EmployeeId = 5, EducationLevel = "Đại học", GeneralEducationLevel = "12/12", SchoolName = "ĐH Kinh tế", Major = "Quản trị nhân lực", GraduationYear = 2015, Ranking = "Khá", ForeignLanguage = "Tiếng Anh TOEIC 700", ITLevel = "Tin học văn phòng", IsPrimary = true },
        new() { Id = 6, EmployeeId = 6, EducationLevel = "Cao đẳng", GeneralEducationLevel = "12/12", SchoolName = "CĐ Kinh tế", Major = "Bán hàng", GraduationYear = 2011, Ranking = "Khá", ForeignLanguage = "Tiếng Anh giao tiếp", ITLevel = "Tin học văn phòng", IsPrimary = true },
        new() { Id = 7, EmployeeId = 7, EducationLevel = "Đại học", GeneralEducationLevel = "12/12", SchoolName = "ĐH Tài chính", Major = "Quản trị kinh doanh", GraduationYear = 2008, Ranking = "Khá", ForeignLanguage = "Tiếng Anh B1", ITLevel = "Nâng cao", IsPrimary = true },
        new() { Id = 8, EmployeeId = 8, EducationLevel = "Đại học", GeneralEducationLevel = "12/12", SchoolName = "ĐH Tài chính", Major = "Kế toán", GraduationYear = 2018, Ranking = "Giỏi", ForeignLanguage = "Tiếng Anh TOEIC 600", ITLevel = "Excel nâng cao", IsPrimary = true }
    ];

    public static IEnumerable<FamilyMember> FamilyMembers =>
    [
        new() { Id = 1, EmployeeId = 4, Relationship = "Bố", FullName = "Nguyễn Văn P", BirthYear = 1965, Occupation = "Kinh doanh", Workplace = "Bình Định", PhoneNumber = "0909000001", IsEmergencyContact = false },
        new() { Id = 2, EmployeeId = 4, Relationship = "Mẹ", FullName = "Trần Thị Q", BirthYear = 1967, Occupation = "Nội trợ", Workplace = "Bình Định", PhoneNumber = "0909000002", IsEmergencyContact = true },
        new() { Id = 3, EmployeeId = 5, Relationship = "Chồng", FullName = "Lê Văn R", BirthYear = 1989, Occupation = "Kỹ sư", Workplace = "TP.HCM", PhoneNumber = "0909000003", IsEmergencyContact = true },
        new() { Id = 4, EmployeeId = 6, Relationship = "Vợ", FullName = "Phạm Thị S", BirthYear = 1990, Occupation = "Kế toán", Workplace = "TP.HCM", PhoneNumber = "0909000004", IsEmergencyContact = true },
        new() { Id = 5, EmployeeId = 7, Relationship = "Con", FullName = "Trần Gia T", BirthYear = 2018, Occupation = "Học sinh", Workplace = "TP.HCM", PhoneNumber = null, IsEmergencyContact = false }
    ];

    public static IEnumerable<WorkHistory> WorkHistories =>
    [
        new() { Id = 1, EmployeeId = 4, CompanyName = "Five Member", PositionTitle = "Nhân viên", StartDate = new DateOnly(2024, 3, 1), EndDate = null, Description = "Phát triển ứng dụng nội bộ" },
        new() { Id = 2, EmployeeId = 5, CompanyName = "Five Member", PositionTitle = "Chuyên viên", StartDate = new DateOnly(2024, 1, 15), EndDate = null, Description = "Quản trị nghiệp vụ HR" },
        new() { Id = 3, EmployeeId = 6, CompanyName = "Five Member", PositionTitle = "Nhân viên", StartDate = new DateOnly(2024, 2, 20), EndDate = null, Description = "Hỗ trợ kinh doanh" },
        new() { Id = 4, EmployeeId = 7, CompanyName = "Five Member", PositionTitle = "Trưởng phòng", StartDate = new DateOnly(2023, 6, 1), EndDate = null, Description = "Quản lý phòng kinh doanh" }
    ];

    public static IEnumerable<Announcement> Announcements =>
    [
        new() { Id = 1, Title = "Lịch nghỉ lễ 30/4 - 1/5", Content = "Toàn công ty nghỉ theo lịch nhà nước.", Priority = "Quan trọng", Status = "Đã đăng", AuthorUserId = 1, CreatedAt = new DateTime(2026, 4, 25, 0, 0, 0, DateTimeKind.Utc) },
        new() { Id = 2, Title = "Cập nhật quy trình chấm công", Content = "Áp dụng check-in mới từ tuần sau.", Priority = "Bình thường", Status = "Đã đăng", AuthorUserId = 2, CreatedAt = new DateTime(2026, 4, 20, 0, 0, 0, DateTimeKind.Utc) },
        new() { Id = 3, Title = "Kế hoạch team building Q2/2026", Content = "Phòng HCNS đang tổng hợp đăng ký.", Priority = "Bình thường", Status = "Bản nháp", AuthorUserId = 2, CreatedAt = new DateTime(2026, 4, 15, 0, 0, 0, DateTimeKind.Utc) },
        new() { Id = 4, Title = "Thay đổi giờ làm việc mùa hè", Content = "Khung giờ mùa hè bắt đầu từ tháng 5.", Priority = "Quan trọng", Status = "Đã đăng", AuthorUserId = 1, CreatedAt = new DateTime(2026, 4, 10, 0, 0, 0, DateTimeKind.Utc) }
    ];
}
