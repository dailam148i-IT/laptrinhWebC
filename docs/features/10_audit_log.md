# Chức năng 10: Audit Log (Nhật ký thay đổi)

> **Controller:** `AuditLogController` (mới)  
> **View:** `Index.cshtml` (mới)  
> **Actor:** 🔐 SA only (Read-only, không sửa/xóa)

---

## Mô tả

Hệ thống ghi lại toàn bộ lịch sử thay đổi dữ liệu (CRUD) và đăng nhập/đăng xuất. Log là **bất biến (immutable)** — chỉ INSERT, không UPDATE/DELETE.

---

## Phân quyền theo Actor

| Hành động | 🔐 SA | 🏢 HR | 👔 MG | 👤 EM |
|-----------|:------:|:------:|:------:|:------:|
| Xem Audit Log | ✅ | ❌ | ❌ | ❌ |
| Tìm kiếm / Lọc log | ✅ | ❌ | ❌ | ❌ |
| Sửa / Xóa log | ❌ | ❌ | ❌ | ❌ |

> Không ai được sửa/xóa log, kể cả Super Admin.

---

## 10.1 Cấu trúc bảng `AuditLogs`

Bảng Append-only (chỉ INSERT).

| Column | Kiểu | Mô tả |
|--------|------|-------|
| `Id` | BIGINT (PK) | Khóa chính, auto-increment |
| `TableName` | VARCHAR(100) | Bảng bị tác động (`Resumes`, `Users`...) |
| `RecordId` | VARCHAR(50) | ID bản ghi bị tác động |
| `Action` | VARCHAR(20) | `INSERT` / `UPDATE` / `DELETE` / `LOGIN` / `LOGOUT` |
| `OldValues` | NVARCHAR(MAX) | JSON trường trước khi sửa (NULL nếu INSERT/LOGIN) |
| `NewValues` | NVARCHAR(MAX) | JSON trường sau khi sửa (NULL nếu DELETE/LOGOUT) |
| `UserId` | INT | ID người thực hiện |
| `IpAddress` | VARCHAR(50) | Địa chỉ IP |
| `Timestamp` | DATETIME | Thời gian thao tác |

---

## 10.2 Trang xem Audit Log (Index)

### Route
- **GET** `/AuditLog`

### Giao diện
- Bảng hiển thị log, sắp xếp theo `Timestamp DESC` (mới nhất trước).
- Hỗ trợ **tìm kiếm** theo: User, Bảng, Action, khoảng thời gian.
- Hỗ trợ **lọc** theo: Action type (INSERT/UPDATE/DELETE/LOGIN/LOGOUT).
- **Phân trang**.
- Xem chi tiết: mở popup/panel hiển thị OldValues vs NewValues dạng diff.

### Yêu cầu
- [ ] Tạo `AuditLogController` với Action `Index`
- [ ] Query `AuditLogs` JOIN `Users` (lấy tên người thực hiện)
- [ ] Tạo View với bảng, filter, phân trang
- [ ] Popup chi tiết diff

---

## 10.3 Ghi Log tự động (EF Interceptor)

### Cách triển khai
Override `SaveChanges()` trong `ApplicationDbContext`:

1. Trước khi save, quét tất cả `ChangeTracker.Entries()`.
2. Với mỗi entity có `State = Modified/Added/Deleted`:
   - Lấy tên bảng (`TableName`).
   - Lấy ID bản ghi (`RecordId`).
   - Lấy giá trị cũ/mới cho các trường thay đổi (chỉ lưu trường thay đổi, không lưu toàn bộ).
   - Tạo `AuditLog` entity.
3. Insert tất cả `AuditLog` trong cùng Transaction.

### Yêu cầu
- [ ] Override `SaveChangesAsync()` trong `ApplicationDbContext`
- [ ] Tạo logic quét `ChangeTracker`
- [ ] Serialize OldValues/NewValues thành JSON

---

## 10.4 Ghi Log Login / Logout

### Vị trí
Ghi trực tiếp trong `AccountController`:
- `Login` (POST thành công) → INSERT AuditLog: Action = `LOGIN`, ghi IP.
- `Logout` → INSERT AuditLog: Action = `LOGOUT`.

### Yêu cầu
- [ ] Inject `ApplicationDbContext` vào `AccountController`
- [ ] Tạo AuditLog entry khi login/logout

---

## 10.5 Bảo mật bảng AuditLogs

### Yêu cầu
- [ ] ConnectionString ứng dụng web: chỉ có quyền `INSERT` + `SELECT` trên bảng `AuditLogs`. KHÔNG có `UPDATE` / `DELETE`.
- [ ] Không tạo API endpoint nào cho phép sửa/xóa log.
- [ ] Retention Policy: lưu trữ tối thiểu 5 năm.

---

### Trạng thái code
❌ Chưa có bất kỳ phần nào. Cần tạo mới: Entity, DbContext override, Controller, View.
