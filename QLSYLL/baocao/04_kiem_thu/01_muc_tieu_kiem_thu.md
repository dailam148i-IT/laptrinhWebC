# 5.4.1. Mục tiêu và Phương pháp kiểm thử

## 1. Mục tiêu kiểm thử

- **Phát hiện lỗi** trong quá trình xây dựng hệ thống trước khi đưa vào sử dụng.
- **Xác minh** các chức năng hoạt động đúng theo đặc tả yêu cầu.
- **Kiểm tra phân quyền** đảm bảo mỗi vai trò chỉ truy cập được chức năng cho phép.
- **Kiểm tra giao diện** responsive trên nhiều kích thước màn hình.

## 2. Phạm vi kiểm thử

| Loại | Phạm vi |
|---|---|
| **Kiểm thử chức năng** | Tất cả CRUD, xác thực, phân quyền, upload file |
| **Kiểm thử phân quyền** | 4 vai trò: SA, HR, Manager, Employee |
| **Kiểm thử giao diện** | Desktop (1920x1080), Tablet (768x1024), Mobile (375x667) |
| **Kiểm thử bảo mật** | SQL Injection, Session hijacking, file upload validation |

## 3. Phương pháp

- **Kiểm thử hộp đen (Black-box):** Kiểm tra input/output mà không cần biết code bên trong.
- **Kiểm thử thủ công:** Thực hiện trên trình duyệt Chrome/Firefox/Edge.
- Mỗi test case gồm: Mục tiêu, Dữ liệu đầu vào, Ràng buộc, Đầu ra dự kiến, Đầu ra thực tế, Kết quả (Pass/Fail).

## 4. Môi trường kiểm thử

| Thành phần | Chi tiết |
|---|---|
| Hệ điều hành | Windows 10/11 |
| Trình duyệt | Chrome, Firefox, Edge (phiên bản mới nhất) |
| Database | SQL Server LocalDB |
| Framework | .NET 9.0 + EF Core |
| Công cụ | Trình duyệt DevTools, Postman (test API) |

## 5. Quy ước ký hiệu

| Ký hiệu | Ý nghĩa |
|---|---|
| ✅ PASS | Test case đạt yêu cầu |
| ❌ FAIL | Test case không đạt, cần fix |
| ⏳ PENDING | Chưa kiểm thử |
| 🔧 FIXED | Đã phát hiện lỗi và đã sửa xong |
