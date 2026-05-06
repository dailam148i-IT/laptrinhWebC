# 5.4.5. Test Cases: Phân quyền RBAC

## 1. Test phân quyền theo vai trò

| TC-ID | Mục tiêu test | Vai trò | Hành động | Đầu ra dự kiến | Đầu ra thực tế | Kết quả |
|---|---|---|---|---|---|---|
| TC-RBAC-01 | SA truy cập Audit Log | Super Admin | GET `/AuditLog/Index` | Hiển thị trang Audit Log | *(Ghi)* | ⏳ |
| TC-RBAC-02 | HR truy cập Audit Log | HR Admin | GET `/AuditLog/Index` | Redirect về Login hoặc 403 | *(Ghi)* | ⏳ |
| TC-RBAC-03 | Manager truy cập Quản lý Phòng ban | Manager | GET `/Department/Index` | Redirect về Login hoặc 403 | *(Ghi)* | ⏳ |
| TC-RBAC-04 | Employee truy cập Quản lý Tài khoản | Employee | GET `/User/Index` | Redirect về Login hoặc 403 | *(Ghi)* | ⏳ |
| TC-RBAC-05 | Employee truy cập Resume/Index | Employee | GET `/Resume/Index` | Redirect về `/Resume/SelfEdit` | *(Ghi)* | ⏳ |
| TC-RBAC-06 | Employee truy cập Resume/Create | Employee | GET `/Resume/Create` | Redirect về Login hoặc 403 | *(Ghi)* | ⏳ |
| TC-RBAC-07 | Manager truy cập Resume/Create | Manager | GET `/Resume/Create` | Redirect về Login hoặc 403 | *(Ghi)* | ⏳ |
| TC-RBAC-08 | Manager xem NV cùng phòng | Manager (IT) | GET `/Resume/Details/1` (NV phòng IT) | Hiển thị chi tiết hồ sơ | *(Ghi)* | ⏳ |
| TC-RBAC-09 | Manager xem NV khác phòng | Manager (IT) | GET `/Resume/Details/5` (NV phòng HR) | Trả về 403 Forbidden | *(Ghi)* | ⏳ |
| TC-RBAC-10 | HR xóa hồ sơ = Soft Delete | HR Admin | POST `/Resume/Delete/1` | IsDeleted=true. NV vẫn còn trong DB. | *(Ghi)* | ⏳ |
| TC-RBAC-11 | SA xóa hồ sơ = Hard Delete | Super Admin | POST `/Resume/Delete/1` | Bản ghi bị xóa vĩnh viễn khỏi DB. | *(Ghi)* | ⏳ |

## 2. Test phân quyền Danh bạ

| TC-ID | Mục tiêu test | Vai trò | Đầu ra dự kiến | Đầu ra thực tế | Kết quả |
|---|---|---|---|---|---|
| TC-RBAC-12 | SA thấy thông tin nhạy cảm | Super Admin | Cột CCCD, Lương, Gia đình hiển thị | *(Ghi)* | ⏳ |
| TC-RBAC-13 | Employee KHÔNG thấy thông tin nhạy cảm | Employee | Cột CCCD, Lương, Gia đình bị ẩn | *(Ghi)* | ⏳ |

## 3. Test phân quyền Tài khoản

| TC-ID | Mục tiêu test | Vai trò | Đầu ra dự kiến | Đầu ra thực tế | Kết quả |
|---|---|---|---|---|---|
| TC-RBAC-14 | HR tạo tài khoản Employee | HR Admin | Tạo thành công | *(Ghi)* | ⏳ |
| TC-RBAC-15 | HR gán role Super Admin | HR Admin | KHÔNG được phép (bị chặn hoặc không hiện option SA) | *(Ghi)* | ⏳ |
| TC-RBAC-16 | SA gán role cho user | Super Admin | Có thể gán bất kỳ role nào | *(Ghi)* | ⏳ |
