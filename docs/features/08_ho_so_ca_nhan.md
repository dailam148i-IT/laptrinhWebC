# Chức năng 8: Hồ sơ cá nhân (Self-Service)

> **Controller:** `ResumeController` (thêm Action mới) hoặc tạo `ProfileController`  
> **View:** `SelfEdit.cshtml` (mới)  
> **Actor:** 👤 EM (chính), 👔 MG (sửa thông tin mình)

---

## Mô tả

Cho phép Employee và Manager tự xem và cập nhật một số thông tin cá nhân trong hồ sơ SYLL **của chính mình**, mà không cần nhờ HR.

---

## Phân quyền theo Actor

| Hành động | 🔐 SA | 🏢 HR | 👔 MG | 👤 EM |
|-----------|:------:|:------:|:------:|:------:|
| Xem hồ sơ cá nhân | ✅ | ✅ | ✅ | ✅ |
| Sửa trường thường | ✅ | ✅ | ✅ | ✅ |
| Sửa trường nhạy cảm* | ✅ | ✅ | ❌ | ❌ |
| Upload ảnh đại diện | ✅ | ✅ | ✅ Ảnh mình | ✅ Ảnh mình |

> *Trường nhạy cảm: Phòng ban, Chức vụ, Số CCCD, Trạng thái, Ngày vào làm.

---

## 8.1 Xem hồ sơ cá nhân

### Route
- **GET** `/Resume/SelfView` hoặc `/Profile`

### Logic
1. Lấy `UserId` từ Session.
2. Query `Resumes` WHERE `UserId = Session.UserId` JOIN `Departments`, `Positions`.
3. Hiển thị toàn bộ thông tin dạng readonly.

---

## 8.2 Sửa hồ sơ cá nhân (Self-Edit)

### Route
- **GET** `/Resume/SelfEdit` hoặc `/Profile/Edit`
- **POST** `/Resume/SelfEdit`

### Trường cho phép MG / EM tự sửa
| Trường | Cho phép sửa | Ghi chú |
|--------|:------------:|---------|
| Số điện thoại | ✅ | |
| Email cá nhân | ✅ | |
| Địa chỉ thường trú | ✅ | |
| Trình độ học vấn | ✅ | Bằng cấp mới đạt được |
| Chuyên ngành | ✅ | |
| Kỹ năng | ✅ | Nếu có trường này |
| Ảnh đại diện | ✅ | Upload mới |

### Trường bị khóa (disabled/hidden) — chỉ SA/HR sửa
| Trường | Lý do |
|--------|-------|
| Họ và tên | Thay đổi cần giấy tờ pháp lý |
| Ngày sinh | Thông tin cố định |
| Giới tính | Thông tin cố định |
| CCCD/CMND | Thông tin nhạy cảm |
| Phòng ban | Chỉ HR/SA điều chuyển |
| Chức vụ | Chỉ HR/SA thăng chức |
| Ngày vào làm | Thông tin hệ thống |
| Trạng thái | Thông tin hệ thống |

### Logic xử lý
1. Lấy `UserId` từ Session → tìm `Resume`.
2. Hiển thị form: trường thường = editable, trường nhạy cảm = disabled (hiển thị nhưng không cho sửa).
3. POST: chỉ cập nhật các trường cho phép. **Bỏ qua** mọi giá trị trường nhạy cảm gửi lên (phòng tấn công qua DevTools).
4. Ghi AuditLog.

### Bảo mật backend
- [ ] Server-side phải có whitelist trường cho phép sửa.
- [ ] Không bind trực tiếp toàn bộ model (`[Bind("Phone,Email,Address,...")]`).
- [ ] Nếu Employee cố gửi `DepartmentId` qua API → bỏ qua hoặc 403.

---

## 8.3 Upload ảnh đại diện

### Route
- **POST** `/Resume/UploadAvatar` hoặc tích hợp trong SelfEdit

### Logic
1. Validate file: `.jpg`, `.png`, `.jpeg`, max 2MB.
2. Kiểm tra `Resume.UserId == Session.UserId` (chỉ upload ảnh mình).
3. Lưu vào `wwwroot/uploads/avatars/{userId}_{timestamp}.ext`.
4. Cập nhật `Resume.AvatarPath`.
5. Ghi AuditLog.

---

### Trạng thái code
❌ Chưa có. Cần tạo mới Controller Action + View.
