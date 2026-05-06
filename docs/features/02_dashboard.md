# Chức năng 2: Dashboard

> **Controller:** `AdminController`  
> **Views:** `Dashboard.cshtml`, `DashboardDept.cshtml` (mới), `DashboardSelf.cshtml` (mới)

---

## 2.1 Dashboard toàn công ty (DASH-ALL)

### Actor: 🔐 Super Admin, 🏢 HR Admin

### Route
- **GET** `/Admin/Dashboard`

### Dữ liệu hiển thị

#### Thẻ thống kê
| Thẻ | Nguồn DB | Mô tả |
|-----|----------|-------|
| Tổng nhân viên | `COUNT(Resumes)` | Toàn bộ nhân viên |
| Tổng phòng ban | `COUNT(Departments)` | Phòng ban đang hoạt động |
| Thông báo mới | `COUNT(Announcements) WHERE gần đây` | Thông báo mới |
| Hồ sơ chờ duyệt | `COUNT(Resumes) WHERE Status='Chờ duyệt'` | Hồ sơ chờ duyệt |

#### Hoạt động gần đây
- Query `AuditLogs` top 5, hiển thị: Mô tả, Thời gian, Màu biểu tượng.

#### Nhân viên mới nhất
- Query `Resumes` JOIN `Departments`, `Positions` ORDER BY `CreatedAt` DESC, top 5.

### Phân quyền
- 🔐 SA: ✅ Truy cập
- 🏢 HR: ✅ Truy cập
- 👔 MG: ❌ Redirect về DashboardDept
- 👤 EM: ❌ Redirect về DashboardSelf

### Trạng thái code
⚠️ UI có, dữ liệu hardcoded, chưa phân quyền.

---

## 2.2 Dashboard phòng ban (DASH-DEPT)

### Actor: 👔 Manager

### Route
- **GET** `/Admin/DashboardDept`

### Mô tả
Hiển thị thống kê và danh sách nhân viên **chỉ trong phòng ban** mà Manager quản lý.

### Dữ liệu hiển thị
| Thẻ | Nguồn DB | Mô tả |
|-----|----------|-------|
| Nhân viên phòng tôi | `COUNT(Resumes) WHERE DepartmentId = Manager.DepartmentId` | Số nhân viên phòng mình |
| Hồ sơ chờ duyệt | `COUNT(Resumes) WHERE Status='Chờ duyệt' AND DepartmentId = ...` | Hồ sơ chờ duyệt phòng mình |
| Thông báo mới | `COUNT(Announcements) WHERE gần đây` | Giống toàn CTy |

#### Danh sách nhân viên phòng ban
- Query `Resumes` WHERE `DepartmentId = Manager.DepartmentId` JOIN `Positions`.
- Hiển thị: Họ tên, Chức vụ, Trạng thái.
- Manager chỉ **xem**, KHÔNG có nút Sửa/Xóa.

### Phân quyền
- 🔐 SA: ✅ (có thể xem bất kỳ phòng nào)
- 🏢 HR: ✅
- 👔 MG: ✅ Chỉ phòng mình
- 👤 EM: ❌ Redirect về DashboardSelf

### Trạng thái code
❌ Chưa có Controller/View.

### Yêu cầu triển khai
- [ ] Thêm Action `DashboardDept()` vào `AdminController`
- [ ] Lấy `DepartmentId` của Manager từ hồ sơ/Session
- [ ] Query dữ liệu theo scope phòng ban
- [ ] Tạo View `DashboardDept.cshtml`

---

## 2.3 Dashboard cá nhân (DASH-SELF)

### Actor: 👤 Employee (mặc định), 👔 MG, 🏢 HR, 🔐 SA (tùy chọn)

### Route
- **GET** `/Admin/DashboardSelf`

### Mô tả
Trang chủ cá nhân cho Employee, hiển thị thông tin bản thân và thông báo mới.

### Dữ liệu hiển thị
| Mục | Nguồn DB | Mô tả |
|-----|----------|-------|
| Thông tin hồ sơ | `Resumes WHERE UserId = Session.UserId` | Họ tên, phòng ban, chức vụ, trạng thái |
| Ảnh đại diện | `Resumes.AvatarPath` | Ảnh cá nhân |
| Thông báo mới nhất | `Announcements` top 5 | Thông báo nội bộ gần đây |
| Trạng thái hồ sơ | `Resumes.Status` | Đã duyệt / Chờ duyệt / Bị khóa |

### Hành động nhanh
- 📝 Cập nhật hồ sơ cá nhân → `/Resume/SelfEdit`
- 🔑 Đổi mật khẩu → `/Account/ChangePassword`
- 📋 Xem danh bạ → `/Contact`

### Phân quyền
- Tất cả Actor đều có thể truy cập trang cá nhân của mình.
- Employee mặc định redirect về đây sau Login.

### Trạng thái code
❌ Chưa có Controller/View.

### Yêu cầu triển khai
- [ ] Thêm Action `DashboardSelf()` vào `AdminController`
- [ ] Query hồ sơ cá nhân theo `UserId` từ Session
- [ ] Query thông báo gần đây
- [ ] Tạo View `DashboardSelf.cshtml`
