# 5.3.3. Chức năng Dashboard

## 1. Tổng quan

Hệ thống cung cấp 3 loại Dashboard khác nhau tuỳ theo vai trò:

| Dashboard | Route | Vai trò |
|---|---|---|
| Toàn công ty | `/Admin/Dashboard` | SA, HR |
| Phòng ban | `/Admin/DashboardDept` | Manager |
| Cá nhân | `/Admin/DashboardSelf` | Employee |

**Controller:** `AdminController.cs`

## 2. Dashboard toàn công ty (SA, HR)

**Dữ liệu hiển thị:**
- 4 card thống kê: Tổng NV, Phòng ban, Thông báo mới (30 ngày), Hồ sơ chờ duyệt
- Bảng "NV mới nhất" (Top 5): Tên, Phòng ban, Chức vụ, Trạng thái
- Timeline "Hoạt động gần đây" (Top 5 AuditLog): Action, Table, RecordId, Thời gian

> **📸 Hình ảnh:** *(Chèn screenshot Dashboard toàn công ty tại đây)*

## 3. Dashboard phòng ban (Manager)

- Chỉ hiển thị NV cùng `DepartmentId` với Manager
- 3 card: Số NV phòng, Chờ duyệt, Thông báo mới
- Bảng danh sách NV phòng ban

> **📸 Hình ảnh:** *(Chèn screenshot Dashboard phòng ban)*

## 4. Dashboard cá nhân (Employee)

- Thông tin hồ sơ cá nhân (Tên, Phòng ban, Chức vụ)
- Top 5 thông báo mới nhất
- Link nhanh đến "Hồ sơ cá nhân"

> **📸 Hình ảnh:** *(Chèn screenshot Dashboard cá nhân)*
