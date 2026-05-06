# 5.4.3. Test Cases: CRUD Hồ sơ SYLL

## 1. Thêm hồ sơ mới

| TC-ID | Mục tiêu test | Dữ liệu đầu vào | Ràng buộc | Đầu ra dự kiến | Đầu ra thực tế | Kết quả |
|---|---|---|---|---|---|---|
| TC-RES-01 | Thêm hồ sơ thành công | Điền đầy đủ: Họ tên, Ngày sinh, Giới tính, SĐT, Phòng ban, Chức vụ, Học vấn | Tất cả trường bắt buộc hợp lệ | Hồ sơ lưu DB với Status="Chờ duyệt". Redirect Index + thông báo thành công. | *(Ghi)* | ⏳ |
| TC-RES-02 | Thiếu Họ tên | FullName = (trống) | Bắt buộc | Validation: "Họ và tên không được để trống" | *(Ghi)* | ⏳ |
| TC-RES-03 | Không còn tài khoản trống | Tất cả User đã có Employee | | Thông báo: "Không còn tài khoản trống để gán hồ sơ mới" | *(Ghi)* | ⏳ |
| TC-RES-04 | Kỹ năng nhiều tags | Skills: "C#, SQL Server, ReactJS" | Phân tách bằng dấu phẩy | 3 bản ghi EmployeeSkill được tạo. Skills mới tự thêm vào bảng Skills. | *(Ghi)* | ⏳ |

## 2. Sửa hồ sơ

| TC-ID | Mục tiêu test | Dữ liệu đầu vào | Ràng buộc | Đầu ra dự kiến | Đầu ra thực tế | Kết quả |
|---|---|---|---|---|---|---|
| TC-RES-05 | Sửa hồ sơ thành công (SA/HR) | Thay đổi FullName, Phone, DepartmentId | Đăng nhập SA/HR | Dữ liệu cập nhật. UpdatedAt ghi nhận. AuditLog có bản ghi UPDATE. | *(Ghi)* | ⏳ |
| TC-RES-06 | Employee tự sửa hồ sơ | Thay đổi Phone, PersonalEmail, CurrentAddress | Đăng nhập Employee | Chỉ 3 trường được cập nhật. Phòng ban, Chức vụ KHÔNG đổi. | *(Ghi)* | ⏳ |
| TC-RES-07 | Sửa hồ sơ không tồn tại | id = 99999 | | Trả về 404 Not Found | *(Ghi)* | ⏳ |

## 3. Xem chi tiết hồ sơ

| TC-ID | Mục tiêu test | Dữ liệu đầu vào | Ràng buộc | Đầu ra dự kiến | Đầu ra thực tế | Kết quả |
|---|---|---|---|---|---|---|
| TC-RES-08 | Xem chi tiết thành công | id hồ sơ hợp lệ | Đăng nhập SA/HR | Hiển thị đầy đủ: Cá nhân, Công việc, Pháp lý, Học vấn, Kỹ năng, Tài liệu, Công tác, Gia đình, Đoàn/Đảng | *(Ghi)* | ⏳ |
| TC-RES-09 | Manager xem NV cùng phòng | Manager IT xem NV thuộc IT | DepartmentId khớp | Hiển thị chi tiết thành công | *(Ghi)* | ⏳ |
| TC-RES-10 | Manager xem NV khác phòng | Manager IT xem NV thuộc HR | DepartmentId KHÔNG khớp | Trả về 403 Forbidden | *(Ghi)* | ⏳ |

## 4. Xóa hồ sơ

| TC-ID | Mục tiêu test | Dữ liệu đầu vào | Ràng buộc | Đầu ra dự kiến | Đầu ra thực tế | Kết quả |
|---|---|---|---|---|---|---|
| TC-RES-11 | HR xóa hồ sơ (Soft Delete) | Đăng nhập HR, xóa NV | Role = HR | IsDeleted=true, DeletedBy ghi UserId, DeletedAt ghi timestamp. NV vẫn trong DB. | *(Ghi)* | ⏳ |
| TC-RES-12 | SA xóa hồ sơ (Hard Delete) | Đăng nhập SA, xóa NV | Role = SA | Bản ghi bị xóa vĩnh viễn khỏi DB. AuditLog ghi DELETE. | *(Ghi)* | ⏳ |

## 5. Tìm kiếm và phân trang

| TC-ID | Mục tiêu test | Dữ liệu đầu vào | Ràng buộc | Đầu ra dự kiến | Đầu ra thực tế | Kết quả |
|---|---|---|---|---|---|---|
| TC-RES-13 | Tìm theo tên | search = "Nguyễn" | | Chỉ hiển thị NV có tên chứa "Nguyễn" | *(Ghi)* | ⏳ |
| TC-RES-14 | Lọc theo phòng ban | departmentId = 1 | | Chỉ hiển thị NV phòng ban có Id=1 | *(Ghi)* | ⏳ |
| TC-RES-15 | Lọc theo trạng thái | status = "Chờ duyệt" | | Chỉ hiển thị NV có Status="Chờ duyệt" | *(Ghi)* | ⏳ |
| TC-RES-16 | Phân trang | page=2, pageSize=5 | Có >5 NV trong DB | Hiển thị bản ghi 6-10. Nút trang 2 active. | *(Ghi)* | ⏳ |
| TC-RES-17 | Không có kết quả | search = "xyz99999" | | Hiển thị bảng trống hoặc thông báo "Không tìm thấy" | *(Ghi)* | ⏳ |
