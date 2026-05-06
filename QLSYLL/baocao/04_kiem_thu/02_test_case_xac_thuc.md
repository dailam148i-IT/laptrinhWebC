# 5.4.2. Test Cases: Xác thực & Đổi mật khẩu

## 1. Module Đăng nhập

| TC-ID | Mục tiêu test | Dữ liệu đầu vào | Ràng buộc | Đầu ra dự kiến | Đầu ra thực tế | Kết quả |
|---|---|---|---|---|---|---|
| TC-AUTH-01 | Đăng nhập thành công (SA) | Username: `admin`, Password: `Admin@123` | Tài khoản tồn tại, IsActive=true | Redirect → `/Admin/Dashboard`. Session được tạo. | *(Ghi kết quả)* | ⏳ |
| TC-AUTH-02 | Đăng nhập thành công (Employee) | Username: `nhanvien01`, Password: mặc định | Tài khoản role Employee | Redirect → `/Admin/DashboardSelf` | *(Ghi kết quả)* | ⏳ |
| TC-AUTH-03 | Sai mật khẩu | Username: `admin`, Password: `sai123` | Username đúng, MK sai | Thông báo: "Tên đăng nhập hoặc mật khẩu không đúng". Không chuyển trang. | *(Ghi kết quả)* | ⏳ |
| TC-AUTH-04 | Để trống Username | Username: (trống), Password: bất kỳ | Validation client-side | Hiển thị validation: "Vui lòng nhập tên đăng nhập" | *(Ghi kết quả)* | ⏳ |
| TC-AUTH-05 | Để trống Password | Username: `admin`, Password: (trống) | Validation client-side | Hiển thị validation: "Vui lòng nhập mật khẩu" | *(Ghi kết quả)* | ⏳ |
| TC-AUTH-06 | Tài khoản bị khóa | Username đúng, MK đúng, nhưng IsActive=false | User.IsActive = false | Thông báo: "Tài khoản đã bị khóa" | *(Ghi kết quả)* | ⏳ |
| TC-AUTH-07 | Đăng xuất | Nhấn nút "Đăng xuất" trên Header | Đã đăng nhập | Session bị xóa. Redirect → `/Account/Login`. AuditLog ghi LOGOUT. | *(Ghi kết quả)* | ⏳ |
| TC-AUTH-08 | Truy cập trang khi chưa đăng nhập | Truy cập trực tiếp `/Resume/Index` | Chưa có Session | Redirect → `/Account/Login` | *(Ghi kết quả)* | ⏳ |
| TC-AUTH-09 | Audit Log ghi nhận đăng nhập | Đăng nhập thành công bất kỳ | | AuditLog có bản ghi: Action=LOGIN, TableName=Users, ghi nhận IP | *(Ghi kết quả)* | ⏳ |

## 2. Module Đổi mật khẩu

| TC-ID | Mục tiêu test | Dữ liệu đầu vào | Ràng buộc | Đầu ra dự kiến | Đầu ra thực tế | Kết quả |
|---|---|---|---|---|---|---|
| TC-PWD-01 | Đổi MK thành công | MK cũ đúng, MK mới hợp lệ, Xác nhận khớp | MK mới ≥ 6 ký tự | Thông báo: "Đổi mật khẩu thành công!". AuditLog ghi PasswordChanged. | *(Ghi kết quả)* | ⏳ |
| TC-PWD-02 | MK cũ sai | MK cũ: `sai123` | | Lỗi: "Mật khẩu hiện tại không đúng" | *(Ghi kết quả)* | ⏳ |
| TC-PWD-03 | Xác nhận MK không khớp | MK mới: `Abc@123`, Xác nhận: `Abc@456` | | Validation: "Xác nhận mật khẩu không khớp" | *(Ghi kết quả)* | ⏳ |
| TC-PWD-04 | MK mới quá ngắn | MK mới: `123` | Tối thiểu 6 ký tự | Validation: "Mật khẩu phải có tối thiểu 6 ký tự" | *(Ghi kết quả)* | ⏳ |

## 3. Module Quên mật khẩu

| TC-ID | Mục tiêu test | Dữ liệu đầu vào | Ràng buộc | Đầu ra dự kiến | Đầu ra thực tế | Kết quả |
|---|---|---|---|---|---|---|
| TC-FG-01 | Quên MK thành công | Username + Email khớp với DB | User tồn tại, chưa bị xóa | Hiển thị MK tạm dạng `Tmp@XXXXXX`. AuditLog ghi ResetPassword. | *(Ghi kết quả)* | ⏳ |
| TC-FG-02 | Username/Email không khớp | Username đúng, Email sai | | Lỗi: "Không tìm thấy tài khoản khớp" | *(Ghi kết quả)* | ⏳ |
| TC-FG-03 | Đăng nhập bằng MK tạm | Dùng MK tạm vừa được cấp | | Đăng nhập thành công | *(Ghi kết quả)* | ⏳ |
