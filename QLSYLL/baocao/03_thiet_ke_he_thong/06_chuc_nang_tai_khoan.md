# 5.3.6. Chức năng Quản lý Tài khoản

**Controller:** `UserController.cs` | **Quyền:** Super Admin, HR Admin

## 1. Danh sách tài khoản (`/User/Index`)
- Bảng: Username, Họ tên, Email, Vai trò, Trạng thái (Hoạt động/Bị khóa), Hành động
- Hỗ trợ tìm kiếm theo tên hoặc username
- Badge màu cho vai trò: SA (đỏ), HR (xanh dương), Manager (vàng), Employee (xám)

> **📸 Hình ảnh:** *(Chèn screenshot danh sách tài khoản)*

## 2. Tạo tài khoản mới (`/User/Create`)
- Trường: Username (UNIQUE), Họ tên, Email (UNIQUE), Vai trò (dropdown), Mật khẩu mặc định
- HR Admin: KHÔNG được gán role Super Admin
- Mật khẩu được hash bằng PasswordHasher trước khi lưu

> **📸 Hình ảnh:** *(Chèn screenshot form tạo tài khoản)*

## 3. Sửa tài khoản (`/User/Edit/{id}`)
- Cho phép sửa: Họ tên, Email, Vai trò, Trạng thái
- Phân quyền Role: chỉ SA mới được gán role SA cho user khác

> **📸 Hình ảnh:** *(Chèn screenshot form sửa tài khoản)*

## 4. Khóa/Mở tài khoản
- Toggle `IsActive`: true ↔ false
- Tài khoản bị khóa (`IsActive = false`) không thể đăng nhập

## 5. Reset mật khẩu
- SA/HR đặt lại mật khẩu cho NV về giá trị mặc định
- Hash mật khẩu mới và cập nhật `PasswordHash`
