# Chức năng 5: Quản lý Chức vụ

> **Controller:** `PositionController`  
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

## 5.1 Danh sách chức vụ (Index)

### Route
- **GET** `/Position`

### Cột hiển thị
| Cột | Mô tả |
|-----|-------|
| `Id` | Mã chức vụ |
| `Name` | Tên chức vụ |
| `Level` | Cấp bậc (1 = cao nhất) |
| `BaseSalary` | Lương cơ bản |
| `EmployeeCount` | Số người giữ chức vụ |
| `Description` | Mô tả ngắn |

### Logic phân quyền
- **SA:** Tất cả + nút CRUD.
- **HR:** Xem, KHÔNG có nút CRUD.
- **MG / EM:** ❌ 403.

---

## 5.2 Thêm chức vụ (Create)

### Actor: 🔐 SA only

### Dữ liệu đầu vào
| Trường | Bắt buộc | Mô tả |
|--------|----------|-------|
| Tên chức vụ | ✅ | Không trùng |
| Cấp bậc | ✅ | Số nguyên, 1 = cao nhất |
| Lương cơ bản | ✅ | Số tiền (VNĐ) |
| Mô tả | ❌ | Chức năng, trách nhiệm |

### Yêu cầu DB
- [ ] Kiểm tra trùng tên
- [ ] Lưu vào `Positions`
- [ ] Ghi AuditLog

---

## 5.3 Chỉnh sửa chức vụ (Edit)

### Actor: 🔐 SA only

### Yêu cầu DB
- [ ] Query + cập nhật `Positions`
- [ ] Ghi AuditLog

---

## 5.4 Xóa chức vụ (Delete)

### Actor: 🔐 SA only

### Ràng buộc
- ❌ Không cho xóa nếu còn nhân viên giữ chức vụ.

### Yêu cầu DB
- [ ] Kiểm tra ràng buộc FK
- [ ] Ghi AuditLog

---

### Trạng thái code
⚠️ UI CRUD có đủ, dùng dữ liệu hardcoded (7 chức vụ), chưa phân quyền.
