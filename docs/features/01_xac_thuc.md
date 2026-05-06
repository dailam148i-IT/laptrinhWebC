# Chức năng 1: Xác thực & Bảo mật

> **Controller:** `AccountController`  
> **Views:** `Login.cshtml`, `ForgotPassword.cshtml`, `ChangePassword.cshtml`  
> **Actor:** 🔐 SA, 🏢 HR, 👔 MG, 👤 EM (tất cả)

---

## 1.1 Đăng nhập (Login)

### Route
| Method | URL | Action |
|--------|-----|--------|
| GET | `/Account/Login` | Hiển thị form đăng nhập |
| POST | `/Account/Login` | Xử lý đăng nhập |

### Dữ liệu đầu vào (LoginViewModel)
| Trường | Kiểu | Bắt buộc | Validation |
|--------|------|----------|------------|
| `Username` | `string` | ✅ | Required |
| `Password` | `string` | ✅ | Required |
| `RememberMe` | `bool` | ❌ | — |

### Logic xử lý
1. Nếu đã đăng nhập (Session có `UserId`) → redirect theo Role.
2. Validate form (`ModelState.IsValid`).
3. Query bảng `Users` theo `Username`, so sánh password hash.
4. Nếu tài khoản `IsActive = false` → báo "Tài khoản đã bị khóa."
5. Nếu đúng → lưu Session: `UserId`, `Username`, `FullName`, `Role` → redirect:
   - **SuperAdmin / HRAdmin** → `/Admin/Dashboard`
   - **Manager** → `/Admin/DashboardDept`
   - **Employee** → `/Admin/DashboardSelf`
6. Nếu sai → báo lỗi, ghi log đăng nhập thất bại.

### Phân quyền theo Actor
| Actor | Redirect sau login |
|-------|--------------------|
| 🔐 Super Admin | Dashboard toàn công ty |
| 🏢 HR Admin | Dashboard toàn công ty |
| 👔 Manager | Dashboard phòng ban |
| 👤 Employee | Dashboard cá nhân |

### Yêu cầu khi kết nối DB
- [ ] Hash mật khẩu (BCrypt hoặc SHA256 + salt)
- [ ] Query bảng `Users` theo `Username`
- [ ] Phân biệt 4 Role để redirect đúng
- [ ] Ghi AuditLog: Action = `LOGIN`, ghi IP
- [ ] Xử lý khóa tài khoản sau N lần sai (tùy chọn)

### Trạng thái code
⚠️ Hardcoded 2 tài khoản, chỉ phân biệt Admin/Employee.

---

## 1.2 Đăng xuất (Logout)

### Route
- **GET** `/Account/Logout`

### Logic xử lý
1. Ghi AuditLog: Action = `LOGOUT`.
2. `HttpContext.Session.Clear()`.
3. Redirect về Login.

### Trạng thái code
✅ Hoạt động, cần bổ sung ghi AuditLog.

---

## 1.3 Quên mật khẩu (ForgotPassword)

### Route
| Method | URL |
|--------|-----|
| GET | `/Account/ForgotPassword` |
| POST | `/Account/ForgotPassword` |

### Dữ liệu đầu vào (ForgotPasswordViewModel)
| Trường | Kiểu | Bắt buộc | Validation |
|--------|------|----------|------------|
| `Username` | `string` | ✅ | Required |
| `Email` | `string` | ✅ | Required, EmailAddress |

### Logic xử lý
1. Validate form.
2. Query `Users` kiểm tra `Username` + `Email` khớp.
3. Tạo mật khẩu mới ngẫu nhiên (hoặc token reset).
4. Gửi qua email (SMTP / SendGrid).
5. Ghi AuditLog.

### Trạng thái code
⚠️ Chưa kiểm tra DB, chưa gửi email.

---

## 1.4 Đổi mật khẩu (ChangePassword)

### Route
| Method | URL |
|--------|-----|
| GET | `/Account/ChangePassword` |
| POST | `/Account/ChangePassword` |

### Dữ liệu đầu vào (ChangePasswordViewModel)
| Trường | Kiểu | Bắt buộc | Validation |
|--------|------|----------|------------|
| `CurrentPassword` | `string` | ✅ | Required |
| `NewPassword` | `string` | ✅ | Required, MinLength(6) |
| `ConfirmPassword` | `string` | ✅ | Required, Compare("NewPassword") |

### Logic xử lý
1. Lấy `UserId` từ Session.
2. So sánh `CurrentPassword` hash với DB.
3. Cập nhật `PasswordHash` mới.
4. Ghi AuditLog.

### Phân quyền
Tất cả 4 Actor đều sử dụng, chỉ đổi mật khẩu **của chính mình**.

### Trạng thái code
⚠️ Chưa kiểm tra mật khẩu cũ từ DB.

---

## 1.5 Middleware phân quyền (Authorization)

### Mô tả
Middleware/Filter kiểm tra quyền truy cập trước khi vào mỗi Action.

### Yêu cầu (chưa có trong code)
- [ ] Tạo `AuthorizationFilter` hoặc dùng ASP.NET Policy-based Authorization.
- [ ] Kiểm tra Session `Role` trước khi cho truy cập Controller/Action.
- [ ] Nếu chưa đăng nhập → redirect về Login.
- [ ] Nếu không đủ quyền → trả 403 Forbidden.
- [ ] Sidebar chỉ hiện menu theo Role:
  - **SA:** Tất cả menu
  - **HR:** Dashboard, Hồ sơ, Thông báo, Tài khoản (hạn chế)
  - **MG:** Dashboard phòng ban, Hồ sơ phòng mình, Danh bạ, Thông báo (xem)
  - **EM:** Dashboard cá nhân, Hồ sơ cá nhân, Danh bạ, Thông báo (xem)

### Trạng thái code
❌ Chưa có. Tất cả Controller đều public, không kiểm tra Role.
