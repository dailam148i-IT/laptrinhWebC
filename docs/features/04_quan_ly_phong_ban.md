# Chức năng 4: Quản lý Phòng ban

> **Controller:** `DepartmentController`  
> **Views:** `Index.cshtml`, `Create.cshtml`, `Edit.cshtml`  
> **Actor:** 🔐 SA (CRUD), 🏢 HR (chỉ xem)

---

## Phân quyền theo Actor

| Hành động | 🔐 SA | 🏢 HR | 👔 MG | 👤 EM |
|-----------|:------:|:------:|:------:|:------:|
| Xem danh sách | ✅ | 👁️ Xem | ❌ | ❌ |
| Tạo mới | ✅ | ❌ | ❌ | ❌ |
| Chỉnh sửa | ✅ | ❌ | ❌ | ❌ |
| Xóa | ✅ | ❌ | ❌ | ❌ |

---

## 4.1 Danh sách phòng ban (Index)

### Route
- **GET** `/Department`

### Cột hiển thị
| Cột | Mô tả |
|-----|-------|
| `Id` | Mã phòng ban |
| `Name` | Tên phòng ban |
| `Code` | Mã viết tắt (KT, NS, KD...) |
| `ManagerName` | Tên trưởng phòng |
| `EmployeeCount` | Tổng nhân viên |
| `CreatedAt` | Ngày thành lập |

### Logic phân quyền
- **SA:** Thấy tất cả + nút Thêm/Sửa/Xóa.
- **HR:** Thấy tất cả, KHÔNG có nút Thêm/Sửa/Xóa.
- **MG / EM:** ❌ 403.

---

## 4.2 Thêm phòng ban (Create)

### Actor: 🔐 SA only

### Route
- **GET** `/Department/Create`
- **POST** `/Department/Create`

### Dữ liệu đầu vào
| Trường | Bắt buộc | Mô tả |
|--------|----------|-------|
| Tên phòng ban | ✅ | Không trùng |
| Mã phòng | ✅ | Unique |
| Trưởng phòng | ❌ | Chọn từ danh sách nhân viên |
| Mô tả | ❌ | Mô tả chức năng |

### Yêu cầu DB
- [ ] Kiểm tra trùng `Code`
- [ ] Lưu vào bảng `Departments`
- [ ] Ghi AuditLog

---

## 4.3 Chỉnh sửa phòng ban (Edit)

### Actor: 🔐 SA only

### Route
- **GET** `/Department/Edit/{id}`
- **POST** `/Department/Edit/{id}`

### Yêu cầu DB
- [ ] Query `Departments` theo `Id`
- [ ] Cập nhật trường thay đổi
- [ ] Ghi AuditLog (OldValues → NewValues)

---

## 4.4 Xóa phòng ban (Delete)

### Actor: 🔐 SA only

### Route
- **POST** `/Department/Delete/{id}`

### Ràng buộc nghiệp vụ
- ❌ Không cho xóa nếu phòng ban còn nhân viên (`EmployeeCount > 0`).
- Hiển thị cảnh báo: "Vui lòng chuyển hết nhân viên sang phòng khác trước khi xóa."

### Yêu cầu DB
- [ ] Kiểm tra ràng buộc FK
- [ ] Soft delete hoặc hard delete
- [ ] Ghi AuditLog

---

### Trạng thái code
⚠️ UI CRUD có đủ, dùng dữ liệu hardcoded (8 phòng ban), chưa phân quyền.
