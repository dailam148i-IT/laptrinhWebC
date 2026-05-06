# 5.3.5. Chức năng Quản lý Phòng ban & Chức vụ

## 1. Quản lý Phòng ban

**Controller:** `DepartmentController.cs` | **Quyền:** Super Admin

### 1.1. Danh sách Phòng ban (`/Department/Index`)
- Hiển thị bảng: Mã P.Ban, Tên, Mô tả, Trưởng phòng, Số NV, Hành động
- Hỗ trợ tìm kiếm theo tên
- Nút Thêm mới, Sửa, Xóa (Soft Delete)

> **📸 Hình ảnh:** *(Chèn screenshot danh sách phòng ban)*

### 1.2. Thêm/Sửa Phòng ban
- Trường: Tên phòng ban, Mã (Code, UNIQUE), Mô tả, Trưởng phòng (dropdown Users có role Manager)
- Validation: Mã không trùng, Tên bắt buộc

> **📸 Hình ảnh:** *(Chèn screenshot form thêm/sửa phòng ban)*

### 1.3. Xóa Phòng ban
- Soft Delete: đánh dấu `IsDeleted = true`
- Kiểm tra: nếu phòng ban còn nhân viên → cảnh báo trước khi xóa

## 2. Quản lý Chức vụ

**Controller:** `PositionController.cs` | **Quyền:** Super Admin

### 2.1. Danh sách Chức vụ (`/Position/Index`)
- Hiển thị bảng: Tên chức vụ, Cấp bậc (Level), Lương cơ bản, Mô tả, Số NV, Hành động
- Sắp xếp theo Level

> **📸 Hình ảnh:** *(Chèn screenshot danh sách chức vụ)*

### 2.2. Thêm/Sửa Chức vụ
- Trường: Tên chức vụ, Cấp bậc (Level), Lương cơ bản (VNĐ), Mô tả
- Lương cơ bản: định dạng số, Precision(18,2)

> **📸 Hình ảnh:** *(Chèn screenshot form thêm/sửa chức vụ)*

### 2.3. Xóa Chức vụ
- Soft Delete: đánh dấu `IsDeleted = true`
