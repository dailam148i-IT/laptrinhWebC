# DANH SÁCH CHỨC NĂNG HỆ THỐNG QLSYLL

> Tài liệu tổng hợp tất cả các chức năng của hệ thống, bao quát đầy đủ 4 Actor.

---

## Actors (Vai trò)

| Ký hiệu | Vai trò | Mô tả |
|----------|---------|-------|
| 🔐 SA | Super Admin | Giám đốc / Quản trị viên IT cấp cao |
| 🏢 HR | HR Admin | Trưởng/Phó phòng Nhân sự, Chuyên viên HR |
| 👔 MG | Manager | Trưởng bộ phận (Trưởng phòng IT, Marketing...) |
| 👤 EM | Employee | Nhân viên cấp dưới |

---

## Danh sách chức năng

| STT | Nhóm chức năng | File tài liệu | Actor liên quan | Trạng thái code |
|-----|----------------|---------------|-----------------|-----------------|
| 1 | Xác thực & Bảo mật | [01_xac_thuc.md](./01_xac_thuc.md) | SA, HR, MG, EM | UI xong, chưa DB |
| 2 | Dashboard | [02_dashboard.md](./02_dashboard.md) | SA, HR, MG, EM | UI 1 phần, thiếu MG + EM |
| 3 | Quản lý Hồ sơ SYLL | [03_quan_ly_ho_so.md](./03_quan_ly_ho_so.md) | SA, HR, MG (xem) | UI xong, chưa phân quyền |
| 4 | Quản lý Phòng ban | [04_quan_ly_phong_ban.md](./04_quan_ly_phong_ban.md) | SA (CRUD), HR (xem) | UI xong, chưa phân quyền |
| 5 | Quản lý Chức vụ | [05_quan_ly_chuc_vu.md](./05_quan_ly_chuc_vu.md) | SA (CRUD), HR (xem) | UI xong, chưa phân quyền |
| 6 | Quản lý Thông báo | [06_quan_ly_thong_bao.md](./06_quan_ly_thong_bao.md) | SA, HR (CRUD) / MG, EM (xem) | UI xong, chưa phân quyền |
| 7 | Quản lý Tài khoản & Phân quyền | [07_quan_ly_tai_khoan.md](./07_quan_ly_tai_khoan.md) | SA (full), HR (hạn chế) | UI xong, chưa phân quyền |
| 8 | Hồ sơ cá nhân (Self-Service) | [08_ho_so_ca_nhan.md](./08_ho_so_ca_nhan.md) | EM, MG | ❌ Chưa có |
| 9 | Danh bạ nội bộ | [09_danh_ba_noi_bo.md](./09_danh_ba_noi_bo.md) | SA, HR, MG, EM | ❌ Chưa có |
| 10 | Audit Log | [10_audit_log.md](./10_audit_log.md) | SA (xem) | ❌ Chưa có |
| 11 | Web API RESTful | [11_web_api.md](./11_web_api.md) | SA | ❌ Chưa có |

---

## Thành phần dùng chung

| Thành phần | Loại | Mô tả |
|------------|------|-------|
| `_AdminLayout.cshtml` | Layout | Bố cục chính cho trang quản trị (sidebar + header + content) |
| `_AuthLayout.cshtml` | Layout | Bố cục cho trang xác thực (Login, Forgot Password) |
| `HeaderViewComponent` | ViewComponent | Header: breadcrumb, chuông thông báo, thông tin user |
| `SidebarViewComponent` | ViewComponent | Menu sidebar — cần cập nhật hiển thị theo Role |
| `_Notification.cshtml` | Partial View | Thông báo Toast (Success, Error, Info) |
| `_StatCard.cshtml` | Partial View | Card thống kê dùng trong Dashboard |

---

## Ma trận phân quyền tổng hợp

| Chức năng | 🔐 SA | 🏢 HR | 👔 MG | 👤 EM |
|-----------|:------:|:------:|:------:|:------:|
| Đăng nhập / Đăng xuất / Đổi MK / Quên MK | ✅ | ✅ | ✅ | ✅ |
| Dashboard toàn công ty | ✅ | ✅ | ❌ | ❌ |
| Dashboard phòng ban | ✅ | ✅ | ✅ | ❌ |
| Dashboard cá nhân | ✅ | ✅ | ✅ | ✅ |
| Xem danh sách hồ sơ (toàn CTy) | ✅ | ✅ | ❌ | ❌ |
| Xem danh sách hồ sơ (phòng mình) | ✅ | ✅ | ✅ | ❌ |
| Xem chi tiết hồ sơ người khác | ✅ | ✅ | 👁️ Phòng mình | ❌ |
| Tạo / Sửa / Xóa hồ sơ | ✅ | ✅ (Xóa: Soft) | ❌ | ❌ |
| Xem/Sửa hồ sơ cá nhân (trường thường) | ✅ | ✅ | ✅ | ✅ |
| Sửa hồ sơ (trường nhạy cảm*) | ✅ | ✅ | ❌ | ❌ |
| Upload ảnh đại diện | ✅ | ✅ | ✅ Ảnh mình | ✅ Ảnh mình |
| Quản lý Phòng ban (CRUD) | ✅ | 👁️ Xem | ❌ | ❌ |
| Quản lý Chức vụ (CRUD) | ✅ | 👁️ Xem | ❌ | ❌ |
| Quản lý Tài khoản (CRUD) | ✅ | ✅ (hạn chế) | ❌ | ❌ |
| Phân quyền Role | ✅ | ❌ | ❌ | ❌ |
| Đăng / Sửa / Xóa Thông báo | ✅ | ✅ | ❌ | ❌ |
| Xem thông báo | ✅ | ✅ | ✅ | ✅ |
| Xem danh bạ nội bộ | ✅ | ✅ | ✅ | ✅ |
| Xem Audit Log | ✅ | ❌ | ❌ | ❌ |
| Web API RESTful | ✅ | ❌ | ❌ | ❌ |

> *Trường nhạy cảm: Phòng ban, Chức vụ, Số CCCD, Trạng thái, Ngày vào làm.
