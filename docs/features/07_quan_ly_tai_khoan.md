# Chức năng 7: Quản lý Tài khoản & Phân quyền

> **Controller:** `UserController`  
> **Views:** `Index.cshtml`, `Create.cshtml`, `Edit.cshtml`  
> **Actor:** 🔐 SA (full + phân quyền Role), 🏢 HR (hạn chế)

---

## Phân quyền theo Actor

| Hành động | 🔐 SA | 🏢 HR | 👔 MG | 👤 EM |
|-----------|:------:|:------:|:------:|:------:|
| Xem danh sách tài khoản | ✅ | ✅ | ❌ | ❌ |
| Tạo tài khoản | ✅ | ✅ (hạn chế) | ❌ | ❌ |
| Chỉnh sửa tài khoản | ✅ | ✅ (hạn chế) | ❌ | ❌ |
| Xóa tài khoản | ✅ | ❌ | ❌ | ❌ |
| Phân quyền Role | ✅ | ❌ | ❌ | ❌ |
| Khóa/Mở khóa | ✅ | ✅ | ❌ | ❌ |
| Reset mật khẩu | ✅ | ✅ | ❌ | ❌ |

### Hạn chế quan trọng của HR Admin
- ❌ **KHÔNG được gán Role = SuperAdmin** cho bất kỳ ai.
- ❌ **KHÔNG được xóa tài khoản** (chỉ SA mới xóa).
- HR chỉ tạo/sửa tài khoản với Role: `Employee`, `Manager`, `HRAdmin`.

---

## 7.1 Danh sách tài khoản (Index)

### Route
- **GET** `/User`

### Cột hiển thị
| Cột | Mô tả |
|-----|-------|
| `Username` | Tên đăng nhập |
| `FullName` | Họ tên |
| `Email` | Email |
| `Role` | SuperAdmin / HRAdmin / Manager / Employee |
| `IsActive` | Đang hoạt động / Bị khóa |
| `CreatedAt` | Ngày tạo |

### Logic phân quyền
- **SA:** Thấy tất cả tài khoản + đầy đủ hành động.
- **HR:** Thấy tất cả, nhưng nút Xóa bị ẩn, dropdown Role không có "SuperAdmin".
- **MG / EM:** ❌ 403.

---

## 7.2 Tạo tài khoản (Create)

### Actor: 🔐 SA, 🏢 HR (hạn chế)

### Dữ liệu đầu vào
| Trường | Bắt buộc | Mô tả |
|--------|----------|-------|
| Tên đăng nhập | ✅ | Unique, không ký tự đặc biệt |
| Mật khẩu | ✅ | Tối thiểu 6 ký tự |
| Xác nhận MK | ✅ | Phải khớp |
| Họ tên | ✅ | Tên hiển thị |
| Email | ✅ | Email hợp lệ, unique |
| Vai trò | ✅ | Dropdown: xem hạn chế bên dưới |

### Logic phân quyền dropdown Role
- **SA tạo:** Dropdown có tất cả 4 Role.
- **HR tạo:** Dropdown chỉ có: Employee, Manager, HRAdmin. **KHÔNG có SuperAdmin.**

### Yêu cầu DB
- [ ] Kiểm tra trùng `Username`, `Email`
- [ ] Hash mật khẩu
- [ ] Lưu vào `Users`
- [ ] Ghi AuditLog

---

## 7.3 Chỉnh sửa tài khoản (Edit)

### Actor: 🔐 SA, 🏢 HR (hạn chế)

### Logic phân quyền
- **HR:** Không được thay đổi Role của tài khoản có Role = SuperAdmin.
- **SA:** Full quyền chỉnh sửa.
- Không cập nhật password ở đây (dùng Reset MK riêng).

### Yêu cầu DB
- [ ] Query + cập nhật `Users`
- [ ] Ghi AuditLog

---

## 7.4 Xóa tài khoản (Delete)

### Actor: 🔐 SA only

### Ràng buộc
- ❌ Không cho xóa tài khoản đang đăng nhập (chính mình).
- ❌ Không cho xóa tài khoản SuperAdmin cuối cùng.
- Soft delete (khuyến nghị).

### Yêu cầu DB
- [ ] Kiểm tra ràng buộc
- [ ] Ghi AuditLog

---

## 7.5 Khóa/Mở khóa (ToggleActive)

### Actor: 🔐 SA, 🏢 HR

### Logic
1. Đảo `IsActive` (true ↔ false).
2. Nếu khóa → buộc đăng xuất Session user đó.
3. Ghi AuditLog.

---

## 7.6 Reset mật khẩu (ResetPassword)

### Actor: 🔐 SA, 🏢 HR

### Mô tả
Admin/HR đặt lại mật khẩu cho user khi họ quên mà không qua email.

### Logic
1. Tạo mật khẩu mới ngẫu nhiên hoặc mật khẩu mặc định.
2. Hash và cập nhật vào DB.
3. Hiển thị mật khẩu mới cho Admin copy/gửi cho user.
4. Ghi AuditLog.

### Trạng thái code
❌ Chưa có Action riêng cho ResetPassword.

---

### Trạng thái code chung
⚠️ UI CRUD có, hardcoded 5 tài khoản, chỉ 2 Role (Admin/Employee), chưa phân quyền.
