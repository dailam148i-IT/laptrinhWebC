# 5.1.1. Mô tả hệ thống và nghiệp vụ cơ bản

## 1. Tên đề tài

**Xây dựng Hệ thống Quản lý Sơ yếu lý lịch nhân sự (QLSYLL)**

## 2. Đặt vấn đề

Trong các doanh nghiệp vừa và nhỏ tại Việt Nam, việc quản lý hồ sơ nhân sự vẫn chủ yếu dựa trên hồ sơ giấy hoặc file Excel phân tán. Điều này dẫn đến nhiều bất cập:

- **Khó tra cứu:** Khi cần tìm thông tin một nhân viên, phải lục tìm trong nhiều tủ hồ sơ hoặc nhiều file khác nhau.
- **Dễ thất lạc:** Hồ sơ giấy có thể bị mất, hư hỏng do thiên tai, cháy nổ.
- **Không kiểm soát được quyền truy cập:** Ai cũng có thể xem, sửa hồ sơ nếu có quyền truy cập thư mục chung.
- **Không có lịch sử thay đổi:** Không biết ai đã sửa thông tin gì, khi nào, gây khó khăn cho kiểm toán nội bộ.
- **Tốn thời gian tổng hợp:** Khi cần báo cáo tổng hợp (số nhân viên theo phòng ban, trình độ học vấn...), phải đếm thủ công.

## 3. Giải pháp đề xuất

Xây dựng một **ứng dụng web** quản lý sơ yếu lý lịch nhân sự tập trung, cho phép:

- Số hóa toàn bộ hồ sơ nhân viên trên nền web.
- Phân quyền rõ ràng theo 4 vai trò: Super Admin, HR Admin, Manager, Employee.
- Ghi nhật ký thay đổi tự động (Audit Trail) để đảm bảo minh bạch.
- Hỗ trợ tìm kiếm, lọc, phân trang nhanh chóng.
- Giao diện responsive, hoạt động trên Desktop, Tablet, Mobile.

## 4. Mô tả tổng quan hệ thống

### 4.1. Kiến trúc hệ thống

Hệ thống được xây dựng theo mô hình **MVC (Model - View - Controller)** trên nền tảng **ASP.NET Core**:

```
┌─────────────┐     ┌──────────────┐     ┌─────────────┐
│   Browser   │────▶│  Controller  │────▶│    Model     │
│  (Client)   │◀────│  (C# Logic)  │◀────│ (EF Core +   │
│  HTML/CSS/JS│     │              │     │  SQL Server) │
└─────────────┘     └──────┬───────┘     └─────────────┘
                           │
                    ┌──────▼───────┐
                    │    View      │
                    │  (Razor      │
                    │   .cshtml)   │
                    └──────────────┘
```

### 4.2. Công nghệ sử dụng

| Thành phần | Công nghệ | Phiên bản |
|---|---|---|
| **Back-end Framework** | ASP.NET Core MVC | .NET 9.0 |
| **ORM** | Entity Framework Core | Code First |
| **Cơ sở dữ liệu** | SQL Server | LocalDB / Express |
| **Front-end** | HTML5, CSS3, JavaScript | - |
| **Icon** | Bootstrap Icons | 1.x |
| **Bảo mật mật khẩu** | BCrypt / HMAC-SHA256 | Custom PasswordHasher |
| **Session** | ASP.NET Core Session | In-Memory |
| **Audit Trail** | EF Core Interceptors | SaveChangesAsync override |

### 4.3. Mô tả nghiệp vụ cơ bản

Hệ thống QLSYLL phục vụ các nghiệp vụ quản lý nhân sự cốt lõi:

#### A. Nghiệp vụ tiếp nhận nhân viên mới
1. HR Admin **tạo tài khoản** mới cho nhân viên (username/mật khẩu mặc định).
2. HR Admin **tạo hồ sơ SYLL** với đầy đủ thông tin cá nhân, học vấn, gia đình.
3. HR Admin **gắn phòng ban và chức vụ** cho nhân viên.
4. Nhân viên đăng nhập → **tự cập nhật ảnh đại diện**, bổ sung kỹ năng, quá trình công tác.
5. Hồ sơ ở trạng thái "Chờ duyệt" cho đến khi HR phê duyệt.

#### B. Nghiệp vụ quản lý hồ sơ hàng ngày
- Nhân viên tự cập nhật SĐT, địa chỉ, email cá nhân khi thay đổi.
- HR Admin cập nhật các trường nhạy cảm (phòng ban, chức vụ, CCCD).
- Manager xem danh sách nhân viên trong phòng ban mình để nắm tình hình nhân sự.

#### C. Nghiệp vụ chuyển phòng ban / thăng chức
1. Manager đề xuất (ngoài hệ thống).
2. HR Admin cập nhật DepartmentId / PositionId trên hồ sơ.
3. Hệ thống **tự động ghi Audit Log** với giá trị cũ/mới.

#### D. Nghiệp vụ nghỉ việc
1. HR Admin cập nhật trạng thái hồ sơ → "Bị khóa".
2. HR Admin khóa tài khoản (IsActive = false).
3. Hồ sơ được **Soft Delete** (không xóa vĩnh viễn, còn tra cứu được).

#### E. Nghiệp vụ thông báo nội bộ
- Super Admin / HR Admin đăng thông báo nội bộ.
- Tất cả nhân viên đều xem được thông báo.
- Hỗ trợ phân loại theo mức độ ưu tiên (Bình thường, Quan trọng, Khẩn cấp).

#### F. Nghiệp vụ kiểm toán (Audit)
- Mọi thao tác INSERT / UPDATE / DELETE đều được ghi log tự động.
- Lưu trữ giá trị cũ/mới dưới dạng JSON.
- Ghi nhận thông tin đăng nhập/đăng xuất (LOGIN / LOGOUT).
- Super Admin là người duy nhất có quyền xem Audit Log.

## 5. Phạm vi đề tài

### 5.1. Trong phạm vi (In Scope)

| # | Chức năng | Mô tả |
|---|---|---|
| 1 | Xác thực & Phân quyền | Đăng nhập, đăng xuất, đổi mật khẩu, quên mật khẩu, RBAC 4 vai trò |
| 2 | Quản lý Hồ sơ SYLL | CRUD hồ sơ nhân viên (thông tin cá nhân, học vấn, gia đình, công tác, kỹ năng, tài liệu) |
| 3 | Quản lý Phòng ban | CRUD phòng ban, gắn trưởng phòng |
| 4 | Quản lý Chức vụ | CRUD chức vụ, mức lương cơ bản |
| 5 | Quản lý Tài khoản | Tạo, khóa/mở, reset mật khẩu, phân quyền |
| 6 | Thông báo nội bộ | Đăng, xem, xóa thông báo |
| 7 | Dashboard | Dashboard toàn công ty, phòng ban, cá nhân |
| 8 | Danh bạ nội bộ | Tìm kiếm thông tin liên lạc nhân viên |
| 9 | Upload file | Ảnh đại diện, tài liệu hồ sơ (PDF, Word, ảnh) |
| 10 | Audit Trail | Ghi log tự động, xem lịch sử thay đổi |

### 5.2. Ngoài phạm vi (Out of Scope)
- Gửi email tự động khi reset mật khẩu (hiện tại hiển thị mật khẩu tạm trên giao diện).
- Xuất báo cáo PDF / Excel.
- Tích hợp hệ thống chấm công, tính lương.
- Chat nội bộ.
