# 5.2.1. Biểu đồ Use-Case

## 1. Use-Case Diagram tổng quan (4 Actors)

```mermaid
graph LR
    SA["🔐 Super Admin"]
    HR["🏢 HR Admin"]
    MG["👔 Manager"]
    EM["👤 Employee"]

    subgraph "Hệ thống QLSYLL"
        UC_AUTH["UC01: Đăng nhập / Đăng xuất"]
        UC_CHPWD["UC02: Đổi mật khẩu"]
        UC_FGPWD["UC03: Quên mật khẩu"]
        UC_DASH_ALL["UC04: Dashboard toàn công ty"]
        UC_DASH_DEPT["UC05: Dashboard phòng ban"]
        UC_DASH_SELF["UC06: Dashboard cá nhân"]
        UC_RES_LIST["UC07: Xem DS hồ sơ"]
        UC_RES_DETAIL["UC08: Xem chi tiết hồ sơ"]
        UC_RES_CREATE["UC09: Tạo hồ sơ mới"]
        UC_RES_EDIT["UC10: Sửa hồ sơ"]
        UC_RES_DEL["UC11: Xóa hồ sơ"]
        UC_SELF_EDIT["UC12: Sửa hồ sơ cá nhân"]
        UC_AVATAR["UC13: Upload ảnh đại diện"]
        UC_DOC["UC14: Upload tài liệu"]
        UC_DEPT["UC15: Quản lý Phòng ban"]
        UC_POS["UC16: Quản lý Chức vụ"]
        UC_USER["UC17: Quản lý Tài khoản"]
        UC_ROLE["UC18: Phân quyền Role"]
        UC_ANN_W["UC19: Đăng / Xóa Thông báo"]
        UC_ANN_R["UC20: Xem thông báo"]
        UC_LOG["UC21: Xem Audit Log"]
        UC_CONTACT["UC22: Danh bạ nội bộ"]
        UC_WORK["UC23: Quá trình công tác"]
    end

    SA --- UC_AUTH
    SA --- UC_CHPWD
    SA --- UC_FGPWD
    SA --- UC_DASH_ALL
    SA --- UC_RES_LIST
    SA --- UC_RES_DETAIL
    SA --- UC_RES_CREATE
    SA --- UC_RES_EDIT
    SA --- UC_RES_DEL
    SA --- UC_DEPT
    SA --- UC_POS
    SA --- UC_USER
    SA --- UC_ROLE
    SA --- UC_ANN_W
    SA --- UC_ANN_R
    SA --- UC_LOG
    SA --- UC_CONTACT

    HR --- UC_AUTH
    HR --- UC_CHPWD
    HR --- UC_FGPWD
    HR --- UC_DASH_ALL
    HR --- UC_RES_LIST
    HR --- UC_RES_DETAIL
    HR --- UC_RES_CREATE
    HR --- UC_RES_EDIT
    HR --- UC_RES_DEL
    HR --- UC_AVATAR
    HR --- UC_DOC
    HR --- UC_USER
    HR --- UC_ANN_W
    HR --- UC_ANN_R
    HR --- UC_CONTACT

    MG --- UC_AUTH
    MG --- UC_CHPWD
    MG --- UC_FGPWD
    MG --- UC_DASH_DEPT
    MG --- UC_RES_LIST
    MG --- UC_RES_DETAIL
    MG --- UC_SELF_EDIT
    MG --- UC_AVATAR
    MG --- UC_ANN_R
    MG --- UC_CONTACT

    EM --- UC_AUTH
    EM --- UC_CHPWD
    EM --- UC_FGPWD
    EM --- UC_DASH_SELF
    EM --- UC_SELF_EDIT
    EM --- UC_AVATAR
    EM --- UC_DOC
    EM --- UC_ANN_R
    EM --- UC_CONTACT
    EM --- UC_WORK
```

## 2. Đặc tả Use-Case chi tiết

### UC01: Đăng nhập

| Thuộc tính | Nội dung |
|---|---|
| **Tên UC** | Đăng nhập hệ thống |
| **Actor** | Super Admin, HR Admin, Manager, Employee |
| **Mô tả** | Người dùng nhập tên đăng nhập và mật khẩu để truy cập hệ thống |
| **Tiền điều kiện** | Tài khoản đã được tạo và đang hoạt động (IsActive = true) |
| **Hậu điều kiện** | Session được tạo, chuyển hướng về Dashboard theo vai trò |
| **Luồng chính** | 1. Người dùng truy cập `/Account/Login` <br> 2. Nhập Username và Password <br> 3. Nhấn "Đăng nhập" <br> 4. Hệ thống kiểm tra Username tồn tại và không bị xóa <br> 5. Hệ thống verify password bằng PasswordHasher <br> 6. Kiểm tra IsActive = true <br> 7. Tạo Session (UserId, Username, FullName, Role) <br> 8. Ghi Audit Log: Action = LOGIN <br> 9. Chuyển hướng: SA/HR → Dashboard, Manager → DashboardDept, Employee → DashboardSelf |
| **Luồng ngoại lệ** | 4a. Username không tồn tại → "Tên đăng nhập hoặc mật khẩu không đúng" <br> 5a. Sai mật khẩu → Thông báo lỗi <br> 6a. Tài khoản bị khóa → "Tài khoản đã bị khóa" |

### UC09: Tạo hồ sơ mới

| Thuộc tính | Nội dung |
|---|---|
| **Tên UC** | Tạo hồ sơ sơ yếu lý lịch mới |
| **Actor** | Super Admin, HR Admin |
| **Mô tả** | Tạo hồ sơ SYLL cho nhân viên mới, tự động gắn với tài khoản chưa có hồ sơ |
| **Tiền điều kiện** | Đã đăng nhập với vai trò SA hoặc HR. Có ít nhất 1 tài khoản chưa được gắn hồ sơ |
| **Hậu điều kiện** | Hồ sơ mới được tạo với trạng thái "Chờ duyệt" |
| **Luồng chính** | 1. Truy cập `/Resume/Create` <br> 2. Điền thông tin cá nhân (Họ tên, Ngày sinh, Giới tính...) <br> 3. Chọn Phòng ban và Chức vụ từ dropdown <br> 4. Điền thông tin học vấn, kỹ năng <br> 5. Nhấn "Lưu" <br> 6. Hệ thống tự tìm tài khoản User chưa có Employee <br> 7. Tạo Employee mới, gắn UserId, Status = "Chờ duyệt" <br> 8. Lưu Education và Skills <br> 9. Chuyển về danh sách + thông báo thành công |
| **Luồng ngoại lệ** | 6a. Không còn tài khoản trống → Thông báo lỗi, redirect về Index |

### UC12: Sửa hồ sơ cá nhân (Employee)

| Thuộc tính | Nội dung |
|---|---|
| **Tên UC** | Nhân viên cập nhật hồ sơ cá nhân |
| **Actor** | Employee, Manager |
| **Mô tả** | Nhân viên tự cập nhật một số trường thông tin cá nhân cho phép |
| **Tiền điều kiện** | Đã đăng nhập. Có hồ sơ Employee gắn với tài khoản |
| **Hậu điều kiện** | Các trường được phép đã được cập nhật |
| **Luồng chính** | 1. Truy cập `/Resume/SelfEdit` <br> 2. Hệ thống load hồ sơ theo UserId trong Session <br> 3. Hiển thị form với các trường: SĐT, Email cá nhân, Địa chỉ, Kỹ năng, Học vấn <br> 4. Nhân viên chỉnh sửa thông tin <br> 5. Nhấn "Cập nhật" <br> 6. Hệ thống chỉ cập nhật: Phone, PersonalEmail, CurrentAddress <br> 7. Lưu Education và Skills <br> 8. Thông báo thành công |
| **Ràng buộc** | KHÔNG cho sửa: FullName, DepartmentId, PositionId, IdentityNumber, JoinDate |

### UC13: Upload ảnh đại diện

| Thuộc tính | Nội dung |
|---|---|
| **Tên UC** | Tải lên ảnh chân dung |
| **Actor** | Tất cả (mỗi người upload ảnh mình; SA/HR upload cho người khác) |
| **Luồng chính** | 1. Chọn file ảnh (JPG/PNG) <br> 2. Hệ thống kiểm tra extension và dung lượng ≤ 2MB <br> 3. Tạo tên file: `{EmployeeId}_{timestamp}.{ext}` <br> 4. Lưu vào `wwwroot/uploads/avatars/` <br> 5. Cập nhật AvatarPath trên Employee <br> 6. Thông báo thành công |
| **Luồng ngoại lệ** | 2a. Sai định dạng → "Chỉ chấp nhận JPG/PNG" <br> 2b. Quá 2MB → "Tối đa 2MB" |

### UC21: Xem Audit Log

| Thuộc tính | Nội dung |
|---|---|
| **Tên UC** | Xem lịch sử thay đổi hệ thống |
| **Actor** | Super Admin |
| **Mô tả** | Xem toàn bộ nhật ký thay đổi dữ liệu trong hệ thống |
| **Tiền điều kiện** | Đăng nhập với vai trò Super Admin |
| **Đặc điểm** | Log là **Append-only** (chỉ INSERT, không UPDATE/DELETE). Hiển thị: Bảng bị tác động, Hành động, Giá trị cũ/mới (JSON), Người thực hiện, IP, Thời gian |

## 3. Use-Case Diagram theo Module

### 3.1. Module Xác thực

```mermaid
graph TD
    subgraph "Module Xác thực"
        UC1["Đăng nhập"]
        UC2["Đăng xuất"]
        UC3["Đổi mật khẩu"]
        UC4["Quên mật khẩu"]
    end

    Actor["Tất cả User"] --- UC1
    Actor --- UC2
    Actor --- UC3
    Actor --- UC4
```

### 3.2. Module Hồ sơ SYLL

```mermaid
graph TD
    subgraph "Module Hồ sơ SYLL"
        UC_LIST["Xem danh sách"]
        UC_DETAIL["Xem chi tiết"]
        UC_CREATE["Tạo mới"]
        UC_EDIT["Chỉnh sửa"]
        UC_DELETE["Xóa"]
        UC_SELF["Sửa hồ sơ cá nhân"]
        UC_AVT["Upload ảnh"]
        UC_DOCU["Upload tài liệu"]
        UC_WORK2["Thêm quá trình công tác"]
    end

    SA2["SA/HR"] --- UC_LIST
    SA2 --- UC_CREATE
    SA2 --- UC_EDIT
    SA2 --- UC_DELETE
    SA2 --- UC_AVT
    SA2 --- UC_DOCU

    MG2["Manager"] --- UC_LIST
    MG2 --- UC_DETAIL
    MG2 --- UC_SELF

    EM2["Employee"] --- UC_SELF
    EM2 --- UC_AVT
    EM2 --- UC_DOCU
    EM2 --- UC_WORK2
```
