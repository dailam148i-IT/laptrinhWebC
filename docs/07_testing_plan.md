# KẾ HOẠCH KIỂM THỬ (Testing Plan)
## Hệ thống Quản lý Sơ yếu lý lịch

---

## 1. Phạm vi kiểm thử
Kiểm thử toàn bộ các chức năng của hệ thống QLSYLL theo 2 mức:
- **Kiểm thử chức năng (Functional Testing):** Đảm bảo mỗi tính năng hoạt động đúng yêu cầu.
- **Kiểm thử giao diện (UI Testing):** Đảm bảo Responsive và trải nghiệm người dùng.

---

## 2. Bảng Test Cases

### 2.1 Module Đăng nhập / Phân quyền

| TC-ID | Tên test case | Bước thực hiện | Dữ liệu đầu vào | Kết quả mong đợi | Trạng thái |
|-------|---------------|----------------|-------------------|-------------------|------------|
| TC-AUTH-01 | Đăng nhập thành công (Admin) | 1. Mở /Account/Login 2. Nhập username/password 3. Nhấn Đăng nhập | username: admin, password: Admin@123 | Chuyển hướng đến /Admin/Dashboard. Session["Role"] = "Admin". | ☐ |
| TC-AUTH-02 | Đăng nhập thành công (Employee) | Tương tự TC-AUTH-01 | username: nhanvien01, password: Nv@123 | Chuyển hướng đến /Employee/Dashboard. Session["Role"] = "Employee". | ☐ |
| TC-AUTH-03 | Đăng nhập sai mật khẩu | Nhập username đúng, password sai | username: admin, password: sai123 | Hiển thị thông báo lỗi "Tên đăng nhập hoặc mật khẩu không đúng". Không chuyển trang. | ☐ |
| TC-AUTH-04 | Đăng nhập thiếu thông tin | Bỏ trống username hoặc password | username: (trống) | Hiển thị Validation message "Vui lòng nhập tên đăng nhập". | ☐ |
| TC-AUTH-05 | Nhớ tài khoản (Cookie) | 1. Đăng nhập với checkbox "Nhớ tài khoản" 2. Đóng trình duyệt 3. Mở lại | Tick checkbox | Trường username được điền sẵn từ Cookie. | ☐ |
| TC-AUTH-06 | Đăng xuất | 1. Nhấn nút Đăng xuất | N/A | Session bị xóa. Chuyển về /Account/Login. | ☐ |
| TC-AUTH-07 | Truy cập trang Admin khi chưa đăng nhập | Truy cập trực tiếp /Resume/Index | N/A | Redirect về /Account/Login. | ☐ |
| TC-AUTH-08 | Employee truy cập trang Admin | Đăng nhập Employee, truy cập /Resume/Create | N/A | Redirect về /Home/AccessDenied hoặc trang lỗi 403. | ☐ |
| TC-AUTH-09 | Đăng nhập tài khoản bị khóa | Nhập đúng username/password nhưng IsActive = false | username: nhanvien02 (bị khóa) | Hiển thị "Tài khoản đã bị khóa. Liên hệ quản trị viên." | ☐ |

### 2.1b Module Đổi mật khẩu

| TC-ID | Tên test case | Bước thực hiện | Dữ liệu đầu vào | Kết quả mong đợi | Trạng thái |
|-------|---------------|----------------|-------------------|-------------------|------------|
| TC-PWD-01 | Đổi mật khẩu thành công | 1. Vào /Account/ChangePassword 2. Nhập đúng mật khẩu cũ 3. Nhập mật khẩu mới hợp lệ | OldPassword: Admin@123, NewPassword: Admin@456 | Mật khẩu cập nhật. Thông báo thành công. | ☐ |
| TC-PWD-02 | Sai mật khẩu cũ | Nhập sai mật khẩu cũ | OldPassword: sai123 | Hiển thị "Mật khẩu cũ không đúng". | ☐ |
| TC-PWD-03 | Mật khẩu mới quá yếu | Nhập mật khẩu mới < 6 ký tự | NewPassword: 123 | Validation: "Mật khẩu phải có tối thiểu 6 ký tự, bao gồm chữ hoa và số". | ☐ |
| TC-PWD-04 | Xác nhận mật khẩu không khớp | NewPassword ≠ ConfirmPassword | NewPassword: Admin@456, Confirm: Admin@789 | Validation: "Xác nhận mật khẩu không khớp". | ☐ |

### 2.1c Module Quản lý Tài khoản (Admin)

| TC-ID | Tên test case | Bước thực hiện | Dữ liệu đầu vào | Kết quả mong đợi | Trạng thái |
|-------|---------------|----------------|-------------------|-------------------|------------|
| TC-USR-01 | Xem danh sách tài khoản | Vào /User/Index | N/A | Hiển thị danh sách Users với Username, FullName, Role, IsActive. | ☐ |
| TC-USR-02 | Tạo tài khoản mới | Nhấn "Tạo mới", nhập thông tin | Username: nhanvien03, FullName: Lê Văn C | Tài khoản được tạo với mật khẩu mặc định. | ☐ |
| TC-USR-03 | Khóa tài khoản | Nhấn nút "Khóa" trên tài khoản nhanvien01 | N/A | IsActive = false. Nhân viên không thể đăng nhập. | ☐ |
| TC-USR-04 | Reset mật khẩu | Nhấn "Reset MK" trên tài khoản | N/A | Mật khẩu đặt lại thành giá trị mặc định. Thông báo thành công. | ☐ |

### 2.2 Module CRUD Hồ sơ SYLL

| TC-ID | Tên test case | Bước thực hiện | Dữ liệu đầu vào | Kết quả mong đợi | Trạng thái |
|-------|---------------|----------------|-------------------|-------------------|------------|
| TC-RES-01 | Thêm hồ sơ thành công | 1. Vào /Resume/Create 2. Điền đầy đủ thông tin 3. Nhấn Lưu | Dữ liệu hợp lệ đầy đủ | Hồ sơ được lưu vào DB. Chuyển về danh sách với thông báo thành công. | ☐ |
| TC-RES-02 | Thêm hồ sơ thiếu trường bắt buộc | Bỏ trống "Họ và tên" | FullName = (trống) | Validation hiện message "Họ và tên không được để trống". Form không submit. | ☐ |
| TC-RES-03 | Thêm hồ sơ trùng CCCD | Nhập CCCD đã tồn tại trong DB | IdentityNumber = 012345678901 (đã có) | Hiển thị lỗi "Số CCCD đã tồn tại trong hệ thống". | ☐ |
| TC-RES-04 | Thêm hồ sơ - Email sai định dạng | Nhập email không hợp lệ | Email = "abc@" | Validation: "Định dạng email không hợp lệ". | ☐ |
| TC-RES-05 | Sửa hồ sơ thành công | 1. Vào /Resume/Edit/{id} 2. Sửa thông tin 3. Nhấn Lưu | Thay đổi FullName | Dữ liệu cập nhật trong DB. UpdatedAt được ghi nhận. | ☐ |
| TC-RES-06 | Xóa hồ sơ | 1. Nhấn nút Xóa trên danh sách 2. Confirm | N/A | Hồ sơ + Education + Experience + Skills + Family bị xóa (CASCADE). | ☐ |
| TC-RES-07 | Xem chi tiết hồ sơ | Nhấn nút "Xem" trên danh sách | N/A | Trang chi tiết hiển thị đầy đủ thông tin: cá nhân, học vấn, kinh nghiệm, kỹ năng, gia đình. | ☐ |

### 2.3 Module Paging, Filtering, AJAX

| TC-ID | Tên test case | Bước thực hiện | Dữ liệu đầu vào | Kết quả mong đợi | Trạng thái |
|-------|---------------|----------------|-------------------|-------------------|------------|
| TC-PAGE-01 | Phân trang trang 1 | Vào /Resume/Index (mặc định) | page=1, pageSize=10 | Hiển thị 10 bản ghi đầu tiên. Nút trang 1 active. | ☐ |
| TC-PAGE-02 | Chuyển trang | Nhấn nút trang 2 | page=2 | Bảng cập nhật bằng AJAX (không reload). Hiển thị bản ghi 11-20. | ☐ |
| TC-FILTER-01 | Tìm kiếm theo tên | Nhập "Nguyễn" vào ô tìm kiếm | search="Nguyễn" | Bảng chỉ hiển thị các hồ sơ có tên chứa "Nguyễn". Tải bằng AJAX. | ☐ |
| TC-FILTER-02 | Lọc theo phòng ban | Chọn "Phòng Kỹ thuật" từ dropdown | departmentId=2 | Bảng chỉ hiển thị nhân viên thuộc Phòng Kỹ thuật. | ☐ |
| TC-FILTER-03 | Tìm kiếm không có kết quả | Nhập "xyz123" | search="xyz123" | Hiển thị thông báo "Không tìm thấy kết quả". | ☐ |

### 2.4 Module Upload Ảnh

| TC-ID | Tên test case | Bước thực hiện | Dữ liệu đầu vào | Kết quả mong đợi | Trạng thái |
|-------|---------------|----------------|-------------------|-------------------|------------|
| TC-IMG-01 | Upload ảnh hợp lệ | Chọn file .jpg, nhấn Upload | avatar.jpg (500KB) | Ảnh được lưu vào /Uploads/Avatars/. Hiển thị trên hồ sơ. | ☐ |
| TC-IMG-02 | Upload file sai định dạng | Chọn file .pdf | document.pdf | Hiển thị lỗi "Chỉ chấp nhận file ảnh JPG, JPEG, PNG". | ☐ |
| TC-IMG-03 | Upload file quá dung lượng | Chọn file > 2MB | bigimage.jpg (5MB) | Hiển thị lỗi "Dung lượng file tối đa 2MB". | ☐ |
| TC-IMG-04 | Preview ảnh trước khi lưu | Chọn file ảnh | avatar.jpg | Ảnh preview hiển thị ngay trên form mà không cần submit. | ☐ |

### 2.5 Module Rich Text Editor

| TC-ID | Tên test case | Bước thực hiện | Dữ liệu đầu vào | Kết quả mong đợi | Trạng thái |
|-------|---------------|----------------|-------------------|-------------------|------------|
| TC-EDITOR-01 | CKEditor hoạt động | Vào form tạo thông báo | N/A | Textarea biến thành CKEditor với toolbar đầy đủ. | ☐ |
| TC-EDITOR-02 | Lưu nội dung Rich Text | Soạn nội dung có in đậm, danh sách, nhấn Lưu | HTML content | Nội dung HTML lưu vào DB. Hiển thị đúng format khi xem. | ☐ |

### 2.6 Module Web API

| TC-ID | Tên test case | Bước thực hiện | Dữ liệu đầu vào | Kết quả mong đợi | Trạng thái |
|-------|---------------|----------------|-------------------|-------------------|------------|
| TC-API-01 | GET /api/resumes | Gửi GET request | N/A | Status 200. JSON chứa danh sách hồ sơ và pagination info. | ☐ |
| TC-API-02 | GET /api/resumes/{id} | Gửi GET với id hợp lệ | id=1 | Status 200. JSON chứa chi tiết hồ sơ. | ☐ |
| TC-API-03 | GET /api/resumes/{id} - ID không tồn tại | Gửi GET với id=99999 | id=99999 | Status 404. JSON: {"success": false, "message": "Không tìm thấy"}. | ☐ |
| TC-API-04 | POST /api/resumes | Gửi POST với body JSON hợp lệ | JSON body đầy đủ | Status 201. Hồ sơ được tạo. | ☐ |
| TC-API-05 | POST /api/resumes - Thiếu dữ liệu | Gửi POST thiếu FullName | JSON thiếu FullName | Status 400. JSON chứa chi tiết lỗi validation. | ☐ |
| TC-API-06 | DELETE /api/resumes/{id} | Gửi DELETE | id=1 | Status 200. Hồ sơ bị xóa khỏi DB. | ☐ |

### 2.7 Kiểm thử Responsive

| TC-ID | Tên test case | Thiết bị / Kích thước | Kết quả mong đợi | Trạng thái |
|-------|---------------|-----------------------|-------------------|------------|
| TC-RES-UI-01 | Desktop 1920x1080 | Chrome Desktop | Layout đầy đủ: Sidebar + Content. Bảng hiển thị đầy đủ cột. | ☐ |
| TC-RES-UI-02 | Tablet 768x1024 | Chrome DevTools (iPad) | Sidebar thu gọn icon. Bảng có scroll ngang nếu cần. | ☐ |
| TC-RES-UI-03 | Mobile 375x667 | Chrome DevTools (iPhone) | Sidebar ẩn, hamburger menu. Form xếp dọc 1 cột. Bảng cuộn ngang. | ☐ |
| TC-RES-UI-04 | Trang Login responsive | Mobile 360x640 | Form đăng nhập căn giữa, không bị tràn hoặc che khuất. | ☐ |

---

## 3. Môi trường kiểm thử
| Thành phần | Chi tiết |
|------------|----------|
| Hệ điều hành | Windows 10/11 |
| Trình duyệt | Chrome (mới nhất), Firefox (mới nhất), Edge (mới nhất) |
| Database | SQL Server LocalDB hoặc SQL Server Express |
| Công cụ test API | Trình duyệt hoặc Postman |

---

## 4. Tiêu chí hoàn thành kiểm thử
- [ ] Tất cả test cases ở mức **PASS** (đánh dấu ✅).
- [ ] Không còn lỗi nghiêm trọng (Critical / High severity).
- [ ] Giao diện hiển thị đúng trên ít nhất 3 kích thước màn hình (Desktop, Tablet, Mobile).
- [ ] API trả về đúng status code và format JSON theo đặc tả.
