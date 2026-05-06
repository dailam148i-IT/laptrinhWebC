# YÊU CẦU KỸ THUẬT
## Hệ thống Quản lý Sơ yếu lý lịch

---

## 1. Kiến trúc hệ thống (System Architecture)

### 1.1 Mô hình MVC
```
┌─────────────────────────────────────────────────┐
│                   CLIENT (Browser)               │
│  HTML5 + CSS3 + Bootstrap 5 + jQuery + AJAX      │
└────────────────────┬────────────────────────────┘
                     │ HTTP Request/Response
┌────────────────────▼────────────────────────────┐
│              ASP.NET MVC APPLICATION             │
│                                                  │
│  ┌──────────┐  ┌──────────┐  ┌──────────────┐   │
│  │  Views   │  │Controllers│  │   Models     │   │
│  │ (Razor)  │◄─┤          ├──►│ (EF Code     │   │
│  │          │  │          │  │   First)      │   │
│  └──────────┘  └────┬─────┘  └──────┬───────┘   │
│                     │               │            │
│              ┌──────▼─────┐         │            │
│              │  Web API   │         │            │
│              │ Controller │         │            │
│              └──────┬─────┘         │            │
│                     │               │            │
└─────────────────────┼───────────────┼────────────┘
                      │               │
               ┌──────▼───────────────▼────────┐
               │        SQL Server Database     │
               └───────────────────────────────┘
```

### 1.2 Cấu trúc thư mục dự án
```
QLNS/
├── docs/                          # Tài liệu dự án
├── QLSYLL/                        # Solution chính
│   ├── Controllers/               # Xử lý Request
│   │   ├── HomeController.cs
│   │   ├── AccountController.cs
│   │   ├── ResumeController.cs
│   │   ├── UserController.cs
│   │   ├── DepartmentController.cs
│   │   ├── PositionController.cs
│   │   ├── AnnouncementController.cs
│   │   └── Api/
│   │       ├── ResumesApiController.cs
│   │       ├── DepartmentsApiController.cs
│   │       └── PositionsApiController.cs
│   ├── Models/                    # Entity + ViewModel
│   │   ├── User.cs
│   │   ├── Resume.cs
│   │   ├── Education.cs
│   │   ├── WorkExperience.cs
│   │   ├── Skill.cs
│   │   ├── FamilyMember.cs
│   │   ├── Department.cs
│   │   ├── Position.cs
│   │   ├── Announcement.cs
│   │   └── ViewModels/
│   │       ├── LoginViewModel.cs
│   │       ├── ChangePasswordViewModel.cs
│   │       ├── ResumeListViewModel.cs
│   │       └── DashboardViewModel.cs
│   ├── Views/                     # Giao diện Razor
│   │   ├── Shared/
│   │   │   ├── _Layout.cshtml     # Layout chung
│   │   │   ├── _AdminLayout.cshtml
│   │   │   └── _Pagination.cshtml # Partial View phân trang
│   │   ├── Home/
│   │   ├── Account/
│   │   ├── Resume/
│   │   ├── Department/
│   │   ├── Position/
│   │   └── Announcement/
│   ├── Content/                   # CSS, Images
│   │   ├── css/
│   │   └── images/
│   ├── Scripts/                   # JavaScript
│   │   └── app/
│   │       ├── resume-list.js     # AJAX cho danh sách hồ sơ
│   │       └── image-upload.js    # Xử lý upload ảnh
│   ├── Uploads/                   # Thư mục lưu file upload
│   │   └── Avatars/
│   ├── DAL/                       # Data Access Layer
│   │   └── AppDbContext.cs        # DbContext
│   ├── Filters/                   # Action Filters
│   │   └── AuthorizeFilter.cs     # Kiểm tra Session/Role
│   ├── App_Start/
│   │   ├── RouteConfig.cs
│   │   └── WebApiConfig.cs
│   └── Web.config
└── QLSYLL.sln                     # Solution file
```

---

## 2. Yêu cầu kỹ thuật chi tiết

### 2.1 Session & Cookies
```
Đăng nhập:
  → Kiểm tra Username/Password trong DB
  → Nếu đúng: Tạo Session["UserId"], Session["FullName"], Session["Role"]
  → Nếu chọn "Nhớ tài khoản": Tạo Cookie("Username") với Expires = 30 ngày

Đăng xuất:
  → Session.Clear()
  → Session.Abandon()
  → Xóa Cookie nếu có

Kiểm tra phân quyền:
  → Tạo Custom Action Filter kiểm tra Session["Role"] trước mỗi Action
  → Nếu chưa đăng nhập → Redirect về /Account/Login
  → Nếu không đủ quyền → Redirect về /Home/AccessDenied

Đổi mật khẩu:
  → Nhận OldPassword, NewPassword, ConfirmPassword
  → Kiểm tra OldPassword có khớp hash trong DB
  → Validate NewPassword (tối thiểu 6 ký tự, có chữ hoa + số)
  → Kiểm tra NewPassword == ConfirmPassword
  → Hash NewPassword rồi cập nhật vào DB
```

### 2.2 Data Annotations & Validation
```csharp
// Ví dụ minh họa cách sử dụng Annotations trên Model Resume
public class Resume
{
    [Required(ErrorMessage = "Họ và tên không được để trống")]
    [StringLength(100, ErrorMessage = "Họ và tên tối đa 100 ký tự")]
    [Display(Name = "Họ và tên")]
    public string FullName { get; set; }

    [Required(ErrorMessage = "Ngày sinh không được để trống")]
    [DataType(DataType.Date)]
    [Display(Name = "Ngày sinh")]
    public DateTime DateOfBirth { get; set; }

    [Required(ErrorMessage = "Số CCCD không được để trống")]
    [RegularExpression(@"^\d{9,12}$", ErrorMessage = "CCCD phải từ 9-12 chữ số")]
    [Display(Name = "Số CCCD/CMND")]
    public string IdentityNumber { get; set; }

    [Required(ErrorMessage = "Email không được để trống")]
    [EmailAddress(ErrorMessage = "Định dạng email không hợp lệ")]
    [Display(Name = "Email")]
    public string Email { get; set; }

    [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
    [Display(Name = "Số điện thoại")]
    public string PhoneNumber { get; set; }
}
```

### 2.3 Paging & Filtering (AJAX)
```
GET /Resume/Index?page=1&pageSize=10&search=Nguyen

Quy trình:
  1. Client gửi AJAX request với tham số page, pageSize, search
  2. Controller nhận request, query DB với Skip/Take
  3. Trả về PartialView chứa bảng dữ liệu + thông tin phân trang
  4. jQuery cập nhật DOM mà không reload toàn trang

Lưu ý:
  - Mặc định: pageSize = 10
  - Tìm kiếm theo: Họ tên, Số CCCD, Email, Phòng ban
```

### 2.4 Upload ảnh
```
Quy trình:
  1. Form sử dụng enctype="multipart/form-data"
  2. Controller nhận HttpPostedFileBase
  3. Validate: chỉ cho phép .jpg/.jpeg/.png, dung lượng <= 2MB
  4. Đổi tên file: {UserId}_{timestamp}.{ext} để tránh trùng
  5. Lưu file vào ~/Uploads/Avatars/
  6. Cập nhật đường dẫn ảnh vào DB
```

### 2.5 Rich Text Editor (CKEditor)
```
Tích hợp CKEditor 5 vào các textarea sau:
  - Mô tả công việc (Work Experience)
  - Nội dung thông báo (Announcement)

Cách tích hợp:
  1. Thêm CDN CKEditor vào _Layout.cshtml
  2. Gắn CKEditor vào <textarea> bằng class selector
  3. Dữ liệu HTML được lưu vào DB dưới dạng Ntext
  4. Hiển thị bằng @Html.Raw() trong View
```

### 2.6 Web API RESTful
```
Base URL: /api/

Endpoints:
  GET    /api/resumes              → Danh sách hồ sơ (hỗ trợ ?page=&search=)
  GET    /api/resumes/{id}         → Chi tiết hồ sơ
  POST   /api/resumes              → Tạo hồ sơ mới (JSON body)
  PUT    /api/resumes/{id}         → Cập nhật hồ sơ (JSON body)
  DELETE /api/resumes/{id}         → Xóa hồ sơ
  GET    /api/departments          → Danh sách phòng ban
  GET    /api/positions            → Danh sách chức vụ

Response format: JSON
HTTP Status codes: 200 (OK), 201 (Created), 400 (Bad Request), 404 (Not Found)
```

### 2.7 Responsive Design
```
Sử dụng Bootstrap 5 Grid System:
  - Desktop (≥992px): Hiển thị đầy đủ sidebar + content
  - Tablet (768px - 991px): Sidebar thu gọn, bảng cuộn ngang nếu cần
  - Mobile (<768px): Sidebar ẩn thành hamburger menu, form xếp dọc

Kiểm tra trên:
  - Chrome DevTools (Device Mode)
  - Desktop: 1920x1080, 1366x768
  - Tablet: 768x1024 (iPad)
  - Mobile: 375x667 (iPhone), 360x640 (Android)
```

---

## 3. Yêu cầu phi chức năng (Non-functional Requirements)
| Yêu cầu | Chi tiết |
|----------|----------|
| **Hiệu năng** | Trang danh sách tải trong < 3 giây. AJAX response < 1 giây. |
| **Bảo mật** | Mật khẩu hash bằng SHA256 hoặc BCrypt. Phòng chống SQL Injection (dùng EF parameterized queries). Chống XSS khi hiển thị dữ liệu Rich Text. |
| **Tương thích** | Chrome, Firefox, Edge (phiên bản mới nhất). |
| **Mã nguồn** | Code sạch, có comment rõ ràng, đặt tên biến/hàm có ý nghĩa. |
