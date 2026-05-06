# Chức năng 3: Quản lý Hồ sơ Sơ yếu lý lịch

> **Controller:** `ResumeController`  
> **Views:** `Index.cshtml`, `Create.cshtml`, `Edit.cshtml`, `Details.cshtml`  
> **Actor chính:** 🔐 SA (full), 🏢 HR (full, xóa soft), 👔 MG (xem phòng mình)

---

## Phân quyền theo Actor

| Hành động | 🔐 SA | 🏢 HR | 👔 MG | 👤 EM |
|-----------|:------:|:------:|:------:|:------:|
| Xem danh sách (toàn CTy) | ✅ | ✅ | ❌ | ❌ |
| Xem danh sách (phòng mình) | ✅ | ✅ | ✅ | ❌ |
| Xem chi tiết | ✅ | ✅ | 👁️ Phòng mình | ❌ |
| Tạo mới | ✅ | ✅ | ❌ | ❌ |
| Chỉnh sửa | ✅ | ✅ | ❌ | ❌ |
| Xóa | ✅ Hard | ⚠️ Soft | ❌ | ❌ |

> 👤 Employee xem/sửa hồ sơ cá nhân ở module riêng → [08_ho_so_ca_nhan.md](./08_ho_so_ca_nhan.md)

---

## 3.1 Danh sách hồ sơ (Index)

### Route
- **GET** `/Resume`

### Cột hiển thị
`Id`, `FullName`, `BirthDate`, `Gender`, `Phone`, `Department`, `Position`, `Status`

### Logic phân quyền
- **SA / HR:** Xem toàn bộ nhân viên.
- **MG:** Xem danh sách chỉ nhân viên cùng `DepartmentId`. Query tự động filter.
- **EM:** ❌ Redirect về Dashboard cá nhân.

### Hành động mỗi dòng
- 🔍 Xem chi tiết → tất cả Actor có quyền xem
- ✏️ Chỉnh sửa → chỉ SA, HR
- 🗑️ Xóa → chỉ SA (hard), HR (soft)

### Yêu cầu DB
- [ ] Query `Resumes` JOIN `Departments`, `Positions`
- [ ] Filter theo `DepartmentId` nếu Role = Manager
- [ ] Tìm kiếm theo tên, phòng ban
- [ ] Lọc theo trạng thái
- [ ] Phân trang

---

## 3.2 Thêm hồ sơ (Create)

### Actor: 🔐 SA, 🏢 HR

### Route
- **GET** `/Resume/Create`
- **POST** `/Resume/Create`

### Dữ liệu đầu vào
| Trường | Bắt buộc | Mô tả |
|--------|----------|-------|
| Họ và tên | ✅ | Tên đầy đủ |
| Ngày sinh | ✅ | Ngày/tháng/năm sinh |
| Giới tính | ✅ | Nam / Nữ |
| SĐT | ✅ | Số điện thoại |
| Email | ❌ | Email cá nhân |
| CCCD/CMND | ✅ | Số căn cước |
| Địa chỉ | ✅ | Địa chỉ thường trú |
| Phòng ban | ✅ | FK → Departments |
| Chức vụ | ✅ | FK → Positions |
| Trình độ | ❌ | Trình độ học vấn |
| Chuyên ngành | ❌ | Chuyên ngành |
| Ảnh đại diện | ❌ | Upload file ảnh |

### Yêu cầu DB
- [ ] Tạo Entity `Resume`
- [ ] Upload ảnh vào `wwwroot/uploads/avatars/`
- [ ] Load dropdown Phòng ban, Chức vụ từ DB
- [ ] Ghi AuditLog (Action = INSERT)

---

## 3.3 Chỉnh sửa hồ sơ (Edit)

### Actor: 🔐 SA, 🏢 HR

### Route
- **GET** `/Resume/Edit/{id}`
- **POST** `/Resume/Edit/{id}`

### Yêu cầu DB
- [ ] Query `Resumes` theo `Id`
- [ ] Cập nhật trường thay đổi
- [ ] Xử lý cập nhật ảnh
- [ ] Ghi AuditLog (OldValues → NewValues)

---

## 3.4 Xem chi tiết (Details)

### Actor: 🔐 SA, 🏢 HR, 👔 MG (phòng mình)

### Route
- **GET** `/Resume/Details/{id}`

### Logic phân quyền
- **MG:** Kiểm tra `Resume.DepartmentId == Manager.DepartmentId`. Nếu không → 403.

### Yêu cầu DB
- [ ] Query `Resumes` JOIN `Departments`, `Positions`

---

## 3.5 Xóa hồ sơ (Delete)

### Route
- **POST** `/Resume/Delete/{id}`

### Logic phân quyền
- **SA:** Hard delete (xóa vĩnh viễn) hoặc duyệt Soft delete của HR.
- **HR:** Soft delete — đặt `IsDeleted = true`, `DeletedAt`, `DeletedBy`. Cần SA duyệt xóa vĩnh viễn.
- **MG / EM:** ❌ 403 Forbidden.

### Yêu cầu DB
- [ ] Soft delete: cập nhật `IsDeleted`, `DeletedAt`, `DeletedBy`
- [ ] Ghi AuditLog

---

## 3.6 Upload ảnh đại diện

### Actor: 🔐 SA, 🏢 HR (bất kỳ), 👔 MG (ảnh mình), 👤 EM (ảnh mình)

### Yêu cầu
- [ ] Xử lý upload file vào `wwwroot/uploads/avatars/`
- [ ] Validate: chỉ cho phép `.jpg`, `.png`, `.jpeg`, max 2MB
- [ ] MG/EM: chỉ upload ảnh cho `ResumeId` của mình

### Trạng thái code
⚠️ View có trường upload nhưng backend chưa xử lý.
