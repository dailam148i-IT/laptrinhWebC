# Chức năng 7: Quản lý Tài khoản & Phân quyền

> **Controller:** `UserController`  
> **Views:** `Index.cshtml`, `Create.cshtml`, `Edit.cshtml`  
> **Actor:** 🔐 SA (quản trị tài khoản quản trị)

---

## Phân quyền theo Actor

| Hành động | 🔐 SA | 🏢 HR | 👔 MG | 👤 EM |
|-----------|:------:|:------:|:------:|:------:|
| Xem danh sách tài khoản | ✅ | ❌ | ❌ | ❌ |
| Tạo tài khoản quản trị `SA/HR` | ✅ | ❌ | ❌ | ❌ |
| Chỉnh sửa tài khoản quản trị `SA/HR` | ✅ | ❌ | ❌ | ❌ |
| Xóa tài khoản | ✅ | ❌ | ❌ | ❌ |
| Phân quyền Role | ✅ | ❌ | ❌ | ❌ |
| Khóa/Mở khóa | ✅ | ❌ | ❌ | ❌ |
| Reset mật khẩu | ✅ | ❌ | ❌ | ❌ |

### Quy ước tạo tài khoản hiện tại
- `UserController.Create/Edit` chỉ xử lý tài khoản quản trị nội bộ `SA` và `HR`.
- Tài khoản `Manager` hoặc `Employee` không được tạo ở module này.
- Tài khoản `Manager/Employee` chỉ được tạo cùng hồ sơ nhân sự tại `ResumeController.Create`.
- Trong danh sách `/User`, tài khoản `Manager/Employee` chỉ hỗ trợ `Khóa/Mở khóa` và `ResetPassword`; không `Edit/Delete` tại đây.

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
- **SA:** Thấy tất cả tài khoản. Tài khoản `SA/HR` có đầy đủ hành động; tài khoản `Manager/Employee` chỉ có `Khóa/Mở khóa` và `ResetPassword`.
- **HR:** ❌ Không truy cập module này trong trạng thái code hiện tại.
- **MG / EM:** ❌ 403.

---

## 7.2 Tạo tài khoản quản trị (Create)

### Actor: 🔐 SA

### Dữ liệu đầu vào
| Trường | Bắt buộc | Mô tả |
|--------|----------|-------|
| Tên đăng nhập | ✅ | Unique, không ký tự đặc biệt |
| Mật khẩu | ✅ | Tối thiểu 6 ký tự |
| Xác nhận MK | ✅ | Phải khớp |
| Họ tên | ✅ | Tên hiển thị |
| Email | ✅ | Email hợp lệ, unique |
| Vai trò | ✅ | Chỉ cho phép `SA` hoặc `HR` |

### Logic phân quyền dropdown Role
- Dropdown chỉ có `SA` và `HR`.
- Nếu sửa HTML để gửi `Manager` hoặc `Employee`, server sẽ từ chối.
- Tài khoản nhân sự phải tạo từ `Resume/Create`.

### Yêu cầu DB
- [ ] Kiểm tra trùng `Username`, `Email`
- [ ] Hash mật khẩu
- [ ] Lưu vào `Users`
- [ ] Ghi AuditLog

---

## 7.3 Chỉnh sửa tài khoản quản trị (Edit)

### Actor: 🔐 SA

### Logic phân quyền
- Chỉ cho sửa tài khoản `SA/HR`.
- Không cho mở màn hình chỉnh sửa đối với tài khoản `Manager/Employee`.
- Không cho đổi nhóm role giữa `SA/HR` và `Manager/Employee`.
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
✅ Đã có action riêng `ResetPassword`.

---

### Trạng thái code chung
✅ Đã tách rõ 2 luồng:
- Tài khoản quản trị `SA/HR` tạo trong `User`
- Tài khoản `Manager/Employee` tạo cùng hồ sơ trong `Resume`
