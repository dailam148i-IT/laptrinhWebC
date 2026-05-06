# 5.3.9. Chức năng Audit Log

**Controller:** `AuditLogController.cs` | **Quyền:** Super Admin

## 1. Mô tả
Hiển thị toàn bộ nhật ký thay đổi dữ liệu trong hệ thống. Audit Log là **Append-only** (chỉ INSERT, không UPDATE/DELETE).

## 2. Cơ chế ghi Log tự động

Audit Log được ghi tự động bằng cách override `SaveChangesAsync()` trong `ApplicationDbContext`:

```
Mỗi khi SaveChanges():
  1. Quét ChangeTracker → tìm entities Added / Modified / Deleted
  2. Với mỗi entity:
     - Added:    NewValues = tất cả properties (JSON)
     - Modified: OldValues + NewValues = chỉ properties thay đổi (JSON)  
     - Deleted:  OldValues = tất cả properties (JSON)
  3. Tạo AuditLog entry: TableName, RecordId, Action, Old/NewValues, UserId, IP, Timestamp
  4. Lưu dữ liệu chính + AuditLog trong cùng transaction
```

## 3. Dữ liệu hiển thị

| # | Cột | Mô tả |
|---|---|---|
| 1 | Thời gian | Timestamp (dd/MM/yyyy HH:mm) |
| 2 | Bảng | TableName (Employees, Users, Departments...) |
| 3 | Hành động | INSERT / UPDATE / DELETE / LOGIN / LOGOUT |
| 4 | ID bản ghi | RecordId |
| 5 | Giá trị cũ | OldValues (JSON) |
| 6 | Giá trị mới | NewValues (JSON) |
| 7 | Người thực hiện | UserId → Tên người dùng |
| 8 | IP | IpAddress |

## 4. Ví dụ Audit Log

| Action | OldValues | NewValues |
|---|---|---|
| LOGIN | NULL | `{"Username":"admin"}` |
| INSERT | NULL | `{"FullName":"Nguyễn Văn A","Phone":"0901234567",...}` |
| UPDATE | `{"Phone":"0901234567"}` | `{"Phone":"0988888888"}` |
| DELETE | `{"FullName":"Nguyễn Văn A",...}` | NULL |

> **📸 Hình ảnh:** *(Chèn screenshot trang Audit Log)*
