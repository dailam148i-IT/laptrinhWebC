# 5.1.3. Phân tích yêu cầu - Chức năng theo lớp người dùng

## 1. Xác định các lớp người dùng (Actors)

Hệ thống QLSYLL phân quyền theo mô hình **RBAC (Role-Based Access Control)** với 4 vai trò:

| # | Vai trò | Mã (Code) | Mô tả | Đối tượng sử dụng |
|---|---|---|---|---|
| 1 | **Super Admin** | `SA` | Quản trị viên hệ thống - toàn quyền | Giám đốc, Quản trị IT |
| 2 | **HR Admin** | `HR` | Quản trị nhân sự | Trưởng/Phó phòng HR, Chuyên viên HR |
| 3 | **Manager** | `Manager` | Trưởng bộ phận | Trưởng phòng các bộ phận |
| 4 | **Employee** | `Employee` | Nhân viên | Tất cả nhân viên |

## 2. Phân tích yêu cầu chức năng theo vai trò

### 2.1. Super Admin (SA)

Super Admin là vai trò cao nhất, có toàn quyền quản trị hệ thống.

**Yêu cầu chức năng:**

| Mã | Chức năng | Mô tả chi tiết | Độ ưu tiên |
|---|---|---|---|
| SA-01 | Dashboard toàn công ty | Xem tổng số nhân viên, phòng ban, thông báo mới, hồ sơ chờ duyệt, hoạt động gần đây | Cao |
| SA-02 | Quản lý hồ sơ SYLL | Xem danh sách, thêm mới, sửa, xóa vĩnh viễn (Hard Delete) hồ sơ toàn công ty | Cao |
| SA-03 | Quản lý Phòng ban | CRUD phòng ban, gắn trưởng phòng, Soft Delete | Cao |
| SA-04 | Quản lý Chức vụ | CRUD chức vụ (tên, level, lương cơ bản), Soft Delete | Cao |
| SA-05 | Quản lý Tài khoản | Tạo tài khoản, phân quyền Role, khóa/mở, reset mật khẩu, xóa | Cao |
| SA-06 | Thông báo nội bộ | Đăng, sửa, xóa thông báo với mức ưu tiên | Trung bình |
| SA-07 | Xem Audit Log | Xem toàn bộ lịch sử thay đổi, lọc theo bảng/hành động/người dùng | Cao |
| SA-08 | Danh bạ nội bộ | Xem toàn bộ thông tin liên lạc (bao gồm thông tin nhạy cảm) | Trung bình |

### 2.2. HR Admin (HR)

HR Admin quản lý nghiệp vụ nhân sự hàng ngày.

**Yêu cầu chức năng:**

| Mã | Chức năng | Mô tả chi tiết | Độ ưu tiên |
|---|---|---|---|
| HR-01 | Dashboard toàn công ty | Tương tự SA nhưng KHÔNG có quyền xem Audit Log | Cao |
| HR-02 | Quản lý hồ sơ SYLL | Xem, thêm, sửa hồ sơ toàn công ty. Xóa = Soft Delete (đánh dấu IsDeleted) | Cao |
| HR-03 | Upload ảnh cho nhân viên | Tải lên/thay ảnh đại diện cho bất kỳ nhân viên nào | Trung bình |
| HR-04 | Quản lý tài liệu hồ sơ | Upload/xóa tài liệu đính kèm (bằng cấp, CCCD, hợp đồng) | Cao |
| HR-05 | Thông báo nội bộ | Đăng, sửa, xóa thông báo | Trung bình |
| HR-06 | Xem danh mục | Xem phòng ban, chức vụ (KHÔNG được tạo/sửa/xóa) | Thấp |

**Điểm khác biệt với Super Admin:**
- ❌ Không được phân quyền Role cho user khác.
- ❌ Không được xem Audit Log.
- ❌ Xóa hồ sơ chỉ là Soft Delete, không xóa vĩnh viễn.
- ❌ Không được quản lý Phòng ban / Chức vụ.

### 2.3. Manager (Trưởng bộ phận)

Manager chỉ có quyền **xem** thông tin nhân sự trong phòng ban mình quản lý.

**Yêu cầu chức năng:**

| Mã | Chức năng | Mô tả chi tiết | Độ ưu tiên |
|---|---|---|---|
| MG-01 | Dashboard phòng ban | Xem số nhân viên phòng mình, hồ sơ chờ duyệt, thông báo mới | Cao |
| MG-02 | Xem danh sách hồ sơ | CHỈ xem danh sách nhân viên thuộc phòng ban mình (`DepartmentId` khớp) | Cao |
| MG-03 | Xem chi tiết hồ sơ | Xem chi tiết hồ sơ nhân viên cùng phòng (KHÔNG được xem phòng khác) | Cao |
| MG-04 | Xem thông báo | Xem danh sách thông báo nội bộ | Trung bình |
| MG-05 | Danh bạ nội bộ | Xem thông tin liên lạc cơ bản toàn công ty | Trung bình |
| MG-06 | Hồ sơ cá nhân | Xem hồ sơ bản thân, cập nhật SĐT, địa chỉ, kỹ năng | Trung bình |

**Ràng buộc bảo mật:**
- ❌ KHÔNG được tạo/sửa/xóa hồ sơ bất kỳ ai.
- ❌ KHÔNG được xem hồ sơ nhân viên phòng ban khác (Server-side check `DepartmentId`).
- ❌ KHÔNG được xem thông tin nhạy cảm (CCCD, lương, gia đình) qua Danh bạ (trừ khi SA/HR).

### 2.4. Employee (Nhân viên)

Employee có quyền hạn thấp nhất, chỉ quản lý hồ sơ cá nhân.

**Yêu cầu chức năng:**

| Mã | Chức năng | Mô tả chi tiết | Độ ưu tiên |
|---|---|---|---|
| EM-01 | Dashboard cá nhân | Xem thông tin bản thân, thông báo mới nhất | Cao |
| EM-02 | Xem/Sửa hồ sơ cá nhân | Chỉ sửa được: SĐT, địa chỉ, email cá nhân, kỹ năng, quá trình công tác | Cao |
| EM-03 | Upload ảnh đại diện | Tải lên/thay ảnh chân dung của bản thân | Trung bình |
| EM-04 | Upload tài liệu | Tải lên tài liệu đính kèm cho hồ sơ bản thân | Trung bình |
| EM-05 | Xem thông báo | Xem danh sách thông báo nội bộ | Trung bình |
| EM-06 | Danh bạ nội bộ | Xem thông tin liên lạc cơ bản của đồng nghiệp | Thấp |
| EM-07 | Đổi mật khẩu | Đổi mật khẩu tài khoản cá nhân | Cao |

**Ràng buộc bảo mật:**
- ❌ KHÔNG được xem hồ sơ người khác.
- ❌ KHÔNG được sửa: Phòng ban, Chức vụ, CCCD, Ngày vào làm.
- ❌ Khi truy cập trang `/Resume/Index` → tự động redirect về `SelfEdit`.

## 3. Ma trận phân quyền tổng hợp (Permission Matrix)

| Chức năng | Super Admin | HR Admin | Manager | Employee |
|---|:---:|:---:|:---:|:---:|
| Đăng nhập / Đăng xuất / Đổi MK | ✅ | ✅ | ✅ | ✅ |
| Quên mật khẩu | ✅ | ✅ | ✅ | ✅ |
| Dashboard toàn công ty | ✅ | ✅ | ❌ | ❌ |
| Dashboard phòng ban | ✅ | ✅ | ✅ | ❌ |
| Dashboard cá nhân | ✅ | ✅ | ✅ | ✅ |
| Xem DS hồ sơ (toàn CTy) | ✅ | ✅ | ❌ | ❌ |
| Xem DS hồ sơ (phòng mình) | ✅ | ✅ | ✅ | ❌ |
| Xem chi tiết hồ sơ người khác | ✅ | ✅ | ✅ *(phòng mình)* | ❌ |
| Tạo hồ sơ mới | ✅ | ✅ | ❌ | ❌ |
| Sửa hồ sơ bất kỳ | ✅ | ✅ | ❌ | ❌ |
| Xóa hồ sơ (Hard Delete) | ✅ | ❌ | ❌ | ❌ |
| Xóa hồ sơ (Soft Delete) | ✅ | ✅ | ❌ | ❌ |
| Sửa hồ sơ cá nhân (SĐT, địa chỉ) | ✅ | ✅ | ✅ | ✅ |
| Upload ảnh cho người khác | ✅ | ✅ | ❌ | ❌ |
| Upload ảnh cá nhân | ✅ | ✅ | ✅ | ✅ |
| Upload tài liệu | ✅ | ✅ | ✅ | ✅ |
| Quản lý Phòng ban (CRUD) | ✅ | ❌ | ❌ | ❌ |
| Quản lý Chức vụ (CRUD) | ✅ | ❌ | ❌ | ❌ |
| Quản lý Tài khoản (Tạo, Khóa, Reset) | ✅ | ✅ | ❌ | ❌ |
| Phân quyền Role | ✅ | ❌ | ❌ | ❌ |
| Đăng / Xóa Thông báo | ✅ | ✅ | ❌ | ❌ |
| Xem thông báo | ✅ | ✅ | ✅ | ✅ |
| Xem Audit Log | ✅ | ❌ | ❌ | ❌ |
| Danh bạ nội bộ | ✅ | ✅ | ✅ | ✅ |
| Xem thông tin nhạy cảm (danh bạ) | ✅ | ✅ | ❌ | ❌ |

## 4. Yêu cầu phi chức năng

| # | Yêu cầu | Mô tả |
|---|---|---|
| NFR-01 | **Bảo mật** | Mật khẩu phải được hash (không lưu plaintext). Session timeout 30 phút. |
| NFR-02 | **Giao diện** | Responsive trên Desktop (1920x1080), Tablet (768x1024), Mobile (375x667). |
| NFR-03 | **Hiệu năng** | Phân trang 5-10 bản ghi/trang, không load toàn bộ dữ liệu. |
| NFR-04 | **Kiểm toán** | Mọi thay đổi dữ liệu đều được ghi log tự động, log là bất biến (Immutable). |
| NFR-05 | **Dung lượng file** | Ảnh đại diện ≤ 2MB (JPG/PNG). Tài liệu ≤ 10MB (PDF/Word/ảnh). |
| NFR-06 | **Trình duyệt** | Hỗ trợ Chrome, Firefox, Edge (phiên bản mới nhất). |
