# Chức năng 6: Quản lý Thông báo

> **Controller:** `AnnouncementController`  
> **Views:** `Index.cshtml`, `Create.cshtml`, `Edit.cshtml`  
> **Actor:** 🔐 SA, 🏢 HR (CRUD) / 👔 MG, 👤 EM (chỉ xem)

---

## Phân quyền theo Actor

| Hành động | 🔐 SA | 🏢 HR | 👔 MG | 👤 EM |
|-----------|:------:|:------:|:------:|:------:|
| Xem danh sách thông báo | ✅ | ✅ | ✅ | ✅ |
| Tạo thông báo | ✅ | ✅ | ❌ | ❌ |
| Chỉnh sửa thông báo | ✅ | ✅ | ❌ | ❌ |
| Xóa thông báo | ✅ | ✅ | ❌ | ❌ |

---

## 6.1 Danh sách thông báo (Index)

### Route
- **GET** `/Announcement`

### Cột hiển thị
| Cột | Mô tả |
|-----|-------|
| `Title` | Tiêu đề |
| `Author` | Người tạo |
| `CreatedAt` | Ngày đăng |
| `Priority` | Quan trọng / Bình thường |
| `Status` | Đã đăng / Bản nháp |

### Logic phân quyền
- **SA / HR:** Thấy tất cả (cả Bản nháp) + nút CRUD.
- **MG / EM:** Chỉ thấy thông báo có `Status = 'Đã đăng'`. Không có nút Sửa/Xóa.

---

## 6.2 Tạo thông báo (Create)

### Actor: 🔐 SA, 🏢 HR

### Dữ liệu đầu vào
| Trường | Bắt buộc | Mô tả |
|--------|----------|-------|
| Tiêu đề | ✅ | Tiêu đề thông báo |
| Nội dung | ✅ | Nội dung (Rich Text Editor) |
| Mức độ ưu tiên | ✅ | Quan trọng / Bình thường |
| Trạng thái | ✅ | Đã đăng / Bản nháp |

### Yêu cầu DB
- [ ] Lưu vào `Announcements`
- [ ] Tự ghi `Author` từ Session, `CreatedAt` = now
- [ ] Ghi AuditLog

---

## 6.3 Chỉnh sửa thông báo (Edit)

### Actor: 🔐 SA, 🏢 HR

### Yêu cầu DB
- [ ] Query + cập nhật `Announcements`
- [ ] Ghi `UpdatedAt` = now
- [ ] Ghi AuditLog

---

## 6.4 Xóa thông báo (Delete)

### Actor: 🔐 SA, 🏢 HR

### Yêu cầu DB
- [ ] Xóa bản ghi `Announcements`
- [ ] Ghi AuditLog

---

### Trạng thái code
⚠️ UI CRUD có đủ, dùng dữ liệu hardcoded (5 thông báo), chưa phân quyền xem.
