# BẢNG THUẬT NGỮ & QUY ƯỚC (Glossary & Conventions)
## Hệ thống Quản lý Sơ yếu lý lịch

---

## 1. Bảng thuật ngữ

| Thuật ngữ | Viết tắt | Ý nghĩa |
|-----------|----------|---------|
| Sơ yếu lý lịch | SYLL | Tài liệu chứa thông tin cá nhân, học vấn, kinh nghiệm của một người |
| Quản lý Sơ yếu lý lịch | QLSYLL | Tên hệ thống đang xây dựng |
| Quản trị viên | Admin | Người có toàn quyền quản lý hệ thống |
| Nhân viên | Employee | Người dùng có quyền hạn chế, chỉ quản lý hồ sơ cá nhân |
| CRUD | - | Create (Tạo), Read (Đọc), Update (Sửa), Delete (Xóa) |
| AJAX | - | Asynchronous JavaScript and XML - Kỹ thuật tải dữ liệu không cần reload trang |
| RESTful | - | Kiến trúc thiết kế API dựa trên HTTP methods |
| ORM | - | Object-Relational Mapping - Ánh xạ đối tượng sang bảng DB |
| Entity Framework | EF | Framework ORM của Microsoft cho .NET |
| Code First | - | Phương pháp tạo DB từ code C# (class → table) |
| Data Annotations | - | Attribute trong C# để kiểm tra dữ liệu (Validation) |
| Razor | - | View engine của ASP.NET MVC, trộn C# và HTML |
| Session | - | Phiên làm việc phía server, lưu trạng thái người dùng |
| Cookie | - | Dữ liệu nhỏ lưu trên trình duyệt người dùng |
| Responsive | - | Giao diện tự động điều chỉnh theo kích thước màn hình |
| CCCD | - | Căn cước công dân |
| Paging | - | Phân trang - chia dữ liệu thành nhiều trang |
| Filtering | - | Lọc dữ liệu theo điều kiện |

---

## 2. Quy ước đặt tên (Naming Conventions)

### 2.1 Database
| Đối tượng | Quy ước | Ví dụ |
|-----------|---------|-------|
| Bảng | PascalCase, số nhiều | `Users`, `Resumes`, `Educations` |
| Cột | PascalCase | `FullName`, `DateOfBirth`, `DepartmentId` |
| Primary Key | TênBảng(số ít) + "Id" | `UserId`, `ResumeId` |
| Foreign Key | TênBảngCha(số ít) + "Id" | `DepartmentId`, `PositionId` |

### 2.2 C# Code
| Đối tượng | Quy ước | Ví dụ |
|-----------|---------|-------|
| Class | PascalCase | `Resume`, `WorkExperience` |
| Method | PascalCase | `GetResumeById()`, `CreateResume()` |
| Property | PascalCase | `FullName`, `DateOfBirth` |
| Biến cục bộ | camelCase | `resumeList`, `currentUser` |
| Controller | TênModule + "Controller" | `ResumeController`, `AccountController` |
| ViewModel | TênChứcNăng + "ViewModel" | `LoginViewModel`, `ResumeListViewModel` |

### 2.3 Views & Files
| Đối tượng | Quy ước | Ví dụ |
|-----------|---------|-------|
| View | PascalCase | `Index.cshtml`, `Create.cshtml`, `Edit.cshtml` |
| Partial View | Bắt đầu bằng "_" | `_Pagination.cshtml`, `_AdminLayout.cshtml` |
| Layout | Bắt đầu bằng "_" | `_Layout.cshtml` |
| CSS file | lowercase, dấu gạch ngang | `site.css`, `admin-style.css` |
| JS file | lowercase, dấu gạch ngang | `resume-list.js`, `image-upload.js` |

---

## 3. Quy ước Git (nếu sử dụng)

### Commit message format:
```
[Loại] Mô tả ngắn gọn

Ví dụ:
[Add] Tạo model Resume với Data Annotations
[Fix] Sửa lỗi phân trang trang cuối cùng
[Update] Cập nhật giao diện form tạo hồ sơ
[Delete] Xóa code thừa trong ResumeController
```

### Branch naming:
```
feature/ten-tinh-nang     → feature/login-session
fix/ten-loi               → fix/paging-error
```
