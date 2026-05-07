# CẤU TRÚC THƯ MỤC DỰ ÁN (CHI TIẾT)
## Hệ thống Quản lý Sơ yếu lý lịch — ASP.NET Core MVC (.NET 10)

---

> **Ghi chú cập nhật:** Tài liệu gốc bên dưới được viết theo cấu trúc MVC 5. Hiện trạng code đang chạy là **ASP.NET Core MVC trên .NET 10** với `Program.cs`, `appsettings.json`, EF Core và session auth tự dựng.
>
> Hai luồng tạo tài khoản chuẩn hiện tại:
> - `UserController.Create/Edit`: chỉ dành cho tài khoản quản trị `SA/HR`
> - `ResumeController.Create`: tạo đồng thời `User + Employee` cho `Manager/Employee`

## 1. Cấu trúc tổng quan

```
D:\Manhinh\QLNS\
│
├── docs\                                    # 📁 Tài liệu dự án
│   ├── 01_project_overview.md
│   ├── 02_business_requirements.md
│   ├── 03_technical_requirements.md
│   ├── 04_database_design.md
│   ├── 05_api_specification.md
│   ├── 06_ui_specification.md
│   ├── 07_testing_plan.md
│   ├── 08_glossary_conventions.md
│   ├── 09_project_structure.md              # (File này)
│   └── DESIGN.md
│
├── QLSYLL.sln                               # 📄 Solution file (mở bằng Visual Studio)
│
└── QLSYLL\                                  # 📁 Project chính (MVC Web Application)
    │
    ├── QLSYLL.csproj                        # File cấu hình project
    ├── Program.cs                           # Entry point, DI, middleware, route mapping
    ├── appsettings.json                     # Connection string, logging, API settings
    │
    ├── App_Start\                           # ⚙️ Cấu hình khởi động
    │   ├── RouteConfig.cs                   #   Đăng ký URL routes cho MVC
    │   ├── WebApiConfig.cs                  #   Đăng ký URL routes cho Web API
    │   ├── FilterConfig.cs                  #   Đăng ký Global Filters
    │   └── BundleConfig.cs                  #   Đóng gói CSS/JS (bundling & minification)
    │
    ├── Models\                              # 📦 Tầng dữ liệu (Entity + ViewModel)
    │   │
    │   ├── User.cs                          #   Entity: Tài khoản người dùng
    │   ├── Resume.cs                        #   Entity: Sơ yếu lý lịch (bảng chính)
    │   ├── Education.cs                     #   Entity: Quá trình học vấn
    │   ├── WorkExperience.cs                #   Entity: Kinh nghiệm làm việc
    │   ├── Skill.cs                         #   Entity: Kỹ năng & Chứng chỉ
    │   ├── FamilyMember.cs                  #   Entity: Thông tin gia đình
    │   ├── Department.cs                    #   Entity: Phòng ban
    │   ├── Position.cs                      #   Entity: Chức vụ
    │   ├── Announcement.cs                  #   Entity: Thông báo nội bộ
    │   │
    │   └── ViewModels\                      #   ViewModel (dữ liệu riêng cho View)
    │       ├── LoginViewModel.cs            #     Form đăng nhập
    │       ├── ChangePasswordViewModel.cs   #     Form đổi mật khẩu
    │       ├── ResumeListViewModel.cs        #     Danh sách hồ sơ + phân trang
    │       ├── ResumeFormViewModel.cs        #     Form tạo/sửa hồ sơ (gộp nhiều bảng)
    │       ├── DashboardViewModel.cs        #     Dữ liệu trang Dashboard
    │       └── UserFormViewModel.cs         #     Form tạo/sửa tài khoản
    │
    ├── DAL\                                 # 💾 Data Access Layer
    │   └── AppDbContext.cs                  #   DbContext — khai báo DbSet, OnModelCreating
    │
    ├── Migrations\                          # 🔄 EF Code First Migrations (tự sinh)
    │   ├── Configuration.cs                 #   Seed data mặc định
    │   └── 202605xx_InitialCreate.cs        #   Migration đầu tiên
    │
    ├── Filters\                             # 🔒 Custom Action Filters
    │   ├── AuthFilter.cs                    #   Kiểm tra đã đăng nhập (Session != null)
    │   └── AdminFilter.cs                   #   Kiểm tra quyền Admin (Session["Role"])
    │
    ├── Helpers\                             # 🛠️ Utility classes
    │   ├── PasswordHelper.cs                #   Hash/Verify mật khẩu (SHA256 hoặc BCrypt)
    │   └── FileHelper.cs                    #   Xử lý upload file (validate, rename, save)
    │
    ├── Controllers\                         # 🎮 Tầng xử lý logic (Controller)
    │   │
    │   ├── HomeController.cs                #   Trang chủ, AccessDenied, Error
    │   ├── AccountController.cs             #   Đăng nhập, Đăng xuất, Đổi mật khẩu
    │   ├── AdminController.cs               #   Dashboard Admin
    │   ├── ResumeController.cs              #   CRUD Hồ sơ SYLL + tạo account Manager/Employee
    │   ├── UserController.cs                #   Quản trị account SA/HR, khóa/mở khóa, reset password
    │   ├── DepartmentController.cs          #   CRUD Phòng ban (Admin)
    │   ├── PositionController.cs            #   CRUD Chức vụ (Admin)
    │   ├── AnnouncementController.cs        #   CRUD Thông báo (Admin)
    │   ├── ContactController.cs             #   Danh bạ nội bộ
    │   └── AuditLogController.cs            #   Nhật ký thao tác
    │
    ├── Views\                               # 🖼️ Tầng giao diện (Razor Views)
    │   │
    │   ├── Web.config                       #   Cấu hình Razor (namespace imports)
    │   ├── _ViewStart.cshtml                #   Khai báo Layout mặc định
    │   │
    │   ├── Shared\                          #   Layout & Partial Views dùng chung
    │   │   ├── _Layout.cshtml               #     Layout chính (Employee)
    │   │   ├── _AdminLayout.cshtml          #     Layout Admin (Sidebar + Header)
    │   │   ├── _Pagination.cshtml           #     Partial: thanh phân trang
    │   │   ├── _Notification.cshtml         #     Partial: thông báo thành công/lỗi
    │   │   └── Error.cshtml                 #     Trang lỗi chung
    │   │
    │   ├── Home\
    │   │   ├── Index.cshtml                 #     Trang chủ (redirect theo role)
    │   │   └── AccessDenied.cshtml          #     Trang không có quyền (403)
    │   │
    │   ├── Account\
    │   │   ├── Login.cshtml                 #     Form đăng nhập
    │   │   └── ChangePassword.cshtml        #     Form đổi mật khẩu
    │   │
    │   ├── Admin\
    │   │   └── Dashboard.cshtml             #     Dashboard Admin (thống kê)
    │   │
    │   ├── Employee\
    │   │   ├── Dashboard.cshtml             #     Dashboard Employee
    │   │   ├── MyResume.cshtml              #     Xem/Sửa hồ sơ cá nhân
    │   │   ├── Directory.cshtml             #     Danh bạ nhân viên
    │   │   └── Announcements.cshtml         #     Xem thông báo
    │   │
    │   ├── Resume\
    │   │   ├── Index.cshtml                 #     Danh sách hồ sơ (Admin)
    │   │   ├── _ResumeTable.cshtml          #     Partial: bảng dữ liệu (AJAX reload)
    │   │   ├── Create.cshtml                #     Form thêm hồ sơ
    │   │   ├── Edit.cshtml                  #     Form sửa hồ sơ
    │   │   └── Details.cshtml               #     Xem chi tiết hồ sơ
    │   │
    │   ├── User\
    │   │   ├── Index.cshtml                 #     Danh sách tài khoản + phân loại admin/employee-linked
    │   │   ├── Create.cshtml                #     Form tạo tài khoản quản trị SA/HR
    │   │   └── Edit.cshtml                  #     Form sửa tài khoản quản trị SA/HR
    │   │
    │   ├── Department\
    │   │   ├── Index.cshtml                 #     Danh sách phòng ban
    │   │   ├── Create.cshtml                #     Form thêm
    │   │   └── Edit.cshtml                  #     Form sửa
    │   │
    │   ├── Position\
    │   │   ├── Index.cshtml                 #     Danh sách chức vụ
    │   │   ├── Create.cshtml                #     Form thêm
    │   │   └── Edit.cshtml                  #     Form sửa
    │   │
    │   └── Announcement\
    │       ├── Index.cshtml                 #     Danh sách thông báo
    │       ├── Create.cshtml                #     Form đăng thông báo (CKEditor)
    │       └── Details.cshtml               #     Xem chi tiết thông báo
    │
    ├── Content\                             # 🎨 Tài nguyên tĩnh — CSS & Hình ảnh
    │   ├── css\
    │   │   ├── site.css                     #     CSS chung toàn trang
    │   │   ├── login.css                    #     CSS riêng trang đăng nhập
    │   │   └── admin.css                    #     CSS riêng layout Admin (sidebar)
    │   ├── images\
    │   │   ├── logo.png                     #     Logo hệ thống
    │   │   ├── default-avatar.png           #     Ảnh mặc định khi chưa upload
    │   │   └── login-bg.jpg                 #     Ảnh nền trang đăng nhập
    │   └── lib\                             #     Thư viện CSS bên thứ 3 (nếu không dùng CDN)
    │       └── bootstrap\                   #       Bootstrap 5 (CSS + JS)
    │
    ├── Scripts\                             # 📜 JavaScript
    │   ├── app\                             #     JS do nhóm tự viết
    │   │   ├── resume-list.js               #       AJAX tải danh sách + phân trang + lọc
    │   │   ├── resume-form.js               #       Thêm/xóa dòng động (Education, Skill...)
    │   │   ├── image-upload.js              #       Preview ảnh trước khi upload
    │   │   └── confirm-delete.js            #       Popup xác nhận xóa
    │   └── lib\                             #     Thư viện JS bên thứ 3 (nếu không dùng CDN)
    │       ├── jquery-3.7.1.min.js          #       jQuery
    │       ├── jquery.validate.min.js       #       jQuery Validation
    │       ├── jquery.validate.unobtrusive.min.js  # Unobtrusive Validation
    │       └── ckeditor\                    #       CKEditor 5 (nếu cài local)
    │
    ├── Uploads\                             # 📤 Thư mục lưu file người dùng upload
    │   └── Avatars\                         #     Ảnh đại diện nhân viên
    │       └── (các file ảnh upload)        #     Ví dụ: 1_20260505.jpg, 2_20260505.png
    │
    ├── fonts\                               # 🔤 Font tùy chỉnh (nếu cần)
    │
    └── bin\                                 # 🔧 Output build (tự sinh, không commit)
```

---

## 2. Giải thích vai trò từng tầng

### 2.1 Luồng xử lý một Request

```
Browser → Route → Controller → Model/DbContext → Database
                       ↓
                 ViewModel ← Query Result
                       ↓
                   View (Razor) → HTML Response → Browser
```

### 2.2 Phân chia trách nhiệm

| Tầng | Thư mục | Trách nhiệm | Ví dụ |
|------|---------|-------------|-------|
| **Routing** | `App_Start\` | Ánh xạ URL → Controller/Action | `/Resume/Index` → `ResumeController.Index()` |
| **Controller** | `Controllers\` | Nhận request, gọi DB, chọn View trả về | `ResumeController.Create(model)` |
| **Model** | `Models\` | Định nghĩa bảng DB + Validation (Annotations) | `[Required] public string FullName` |
| **ViewModel** | `Models\ViewModels\` | Dữ liệu chuyên biệt cho View (không phải entity) | `ResumeListViewModel { List, PageInfo }` |
| **View** | `Views\` | Giao diện HTML + Razor syntax | `@Html.TextBoxFor(m => m.FullName)` |
| **Partial View** | `Views\Shared\` | Component dùng lại nhiều nơi | `_Pagination.cshtml` |
| **DbContext** | `DAL\` | Kết nối DB, khai báo bảng, cấu hình EF | `public DbSet<Resume> Resumes { get; set; }` |
| **Filter** | `Filters\` | Kiểm tra phân quyền trước mỗi Action | `[AuthFilter]`, `[AdminFilter]` |
| **Helper** | `Helpers\` | Logic tiện ích dùng chung | `PasswordHelper.Hash("abc")` |
| **API** | `Controllers\Api\` | Endpoint RESTful trả JSON | `GET /api/resumes → JSON` |
| **Static** | `Content\`, `Scripts\` | CSS, JS, hình ảnh | `site.css`, `resume-list.js` |

---

## 3. Các file quan trọng cần lưu ý

### 3.1 Web.config (Connection String)
```xml
<connectionStrings>
  <add name="AppDbContext"
       connectionString="Server=(localdb)\MSSQLLocalDB;Database=QLSYLL;
                          Integrated Security=True;MultipleActiveResultSets=True"
       providerName="System.Data.SqlClient" />
</connectionStrings>
```

### 3.2 Global.asax.cs (Đăng ký khi app khởi động)
```csharp
protected void Application_Start()
{
    AreaRegistration.RegisterAllAreas();
    GlobalConfiguration.Configure(WebApiConfig.Register);  // Web API
    FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
    RouteConfig.RegisterRoutes(RouteTable.Routes);
    BundleConfig.RegisterBundles(BundleTable.Bundles);
}
```

### 3.3 RouteConfig.cs (URL Routing)
```csharp
routes.MapRoute(
    name: "Default",
    url: "{controller}/{action}/{id}",
    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
);
```

### 3.4 WebApiConfig.cs (API Routing)
```csharp
config.Routes.MapHttpRoute(
    name: "DefaultApi",
    routeTemplate: "api/{controller}/{id}",
    defaults: new { id = RouteParameter.Optional }
);
```

---

## 4. Thống kê số lượng file dự kiến

| Loại | Số lượng | Ghi chú |
|------|----------|---------|
| Entity Models | 9 | User, Resume, Education, WorkExperience, Skill, FamilyMember, Department, Position, Announcement |
| ViewModels | 6 | Login, ChangePassword, ResumeList, ResumeForm, Dashboard, UserForm |
| Controllers (MVC) | 9 | Home, Account, Admin, Employee, Resume, User, Department, Position, Announcement |
| Controllers (API) | 3 | ResumesApi, DepartmentsApi, PositionsApi |
| Views (.cshtml) | ~28 | Bao gồm cả Partial Views |
| Layouts | 2 | _Layout (Employee), _AdminLayout (Admin + Sidebar) |
| CSS files | 3 | site.css, login.css, admin.css |
| JS files (custom) | 4 | resume-list.js, resume-form.js, image-upload.js, confirm-delete.js |
| Filters | 2 | AuthFilter, AdminFilter |
| Helpers | 2 | PasswordHelper, FileHelper |
| **Tổng cộng** | **~68 files** | Không tính thư viện bên thứ 3 |
