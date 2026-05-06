# TỔNG QUAN DỰ ÁN
## Hệ thống Quản lý Sơ yếu lý lịch (Resume Management System)

---

## 1. Giới thiệu
Hệ thống Quản lý Sơ yếu lý lịch (QLSYLL) là một ứng dụng web được xây dựng trên nền tảng **ASP.NET MVC**, cho phép tổ chức/doanh nghiệp quản lý hồ sơ sơ yếu lý lịch của nhân viên một cách tập trung, hiệu quả và chuyên nghiệp.

## 2. Mục tiêu dự án
- **Số hóa** quy trình quản lý sơ yếu lý lịch, thay thế hồ sơ giấy truyền thống.
- Cung cấp giao diện **thân thiện, responsive** để người dùng có thể truy cập trên mọi thiết bị.
- Đảm bảo **phân quyền rõ ràng** giữa Quản trị viên (Admin) và Nhân viên (Employee).
- Hỗ trợ **tìm kiếm, lọc, phân trang** nhanh chóng trên lượng dữ liệu lớn.
- Tích hợp **Web API RESTful** để mở rộng và kết nối với các hệ thống khác.

## 3. Phạm vi dự án
### 3.1 Trong phạm vi (In Scope)
- Quản lý thông tin cá nhân, học vấn, kinh nghiệm làm việc, kỹ năng, thành viên gia đình.
- Chức năng đăng nhập/đăng xuất với Session & Cookies.
- CRUD đầy đủ trên các bảng dữ liệu chính.
- Upload ảnh đại diện cho hồ sơ.
- Tích hợp trình soạn thảo văn bản (Rich Text Editor) cho các trường mô tả.
- API RESTful phục vụ truy vấn dữ liệu hồ sơ.
- Giao diện responsive trên Desktop, Tablet, Mobile.

### 3.2 Ngoài phạm vi (Out of Scope)
- Tích hợp hệ thống email tự động.
- Chức năng chat nội bộ.
- Xuất báo cáo PDF (có thể bổ sung nếu đủ thời gian).

## 4. Công nghệ sử dụng
| Thành phần    | Công nghệ                          |
|---------------|-------------------------------------|
| Back-end      | ASP.NET MVC 5 (.NET Framework 4.8) |
| ORM           | Entity Framework (Code First)       |
| Cơ sở dữ liệu| SQL Server                          |
| Front-end     | HTML5, CSS3, Bootstrap 5, jQuery    |
| AJAX          | jQuery AJAX                         |
| Rich Editor   | CKEditor 5 hoặc TinyMCE            |
| Validation    | Data Annotations + jQuery Validate  |
| API           | ASP.NET Web API (RESTful)           |

## 5. Đội ngũ phát triển
| STT | Họ và tên | Vai trò | Nhiệm vụ chính |
|-----|-----------|---------|-----------------|
| 1   | (Điền tên)| Trưởng nhóm | Phân tích, thiết kế DB, quản lý tiến độ |
| 2   | (Điền tên)| Back-end Dev | Viết Controllers, Models, API |
| 3   | (Điền tên)| Front-end Dev | Thiết kế Views, Layout, AJAX |
| 4   | (Điền tên)| Tester | Kiểm thử, viết tài liệu |
| 5   | (Điền tên)| Full-stack | Hỗ trợ Back-end & Front-end |

## 6. Mô hình phát triển
Áp dụng mô hình **Waterfall (Thác nước)** với các giai đoạn:
1. Phân tích yêu cầu → 2. Thiết kế → 3. Lập trình → 4. Kiểm thử → 5. Triển khai

## 7. Kế hoạch thời gian (Timeline)
| Giai đoạn | Công việc | Thời gian dự kiến |
|-----------|-----------|-------------------|
| Tuần 1-2  | Phân tích yêu cầu, thiết kế DB | 2 tuần |
| Tuần 3-5  | Lập trình Core (Login, CRUD, phân quyền) | 3 tuần |
| Tuần 6-7  | Tính năng nâng cao (AJAX, Paging, Upload, API) | 2 tuần |
| Tuần 8    | Kiểm thử, sửa lỗi, hoàn thiện tài liệu | 1 tuần |
