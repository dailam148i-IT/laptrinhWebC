# 5.4.4. Test Cases: Upload Ảnh & Tài liệu

## 1. Upload ảnh đại diện

| TC-ID | Mục tiêu test | Dữ liệu đầu vào | Ràng buộc | Đầu ra dự kiến | Đầu ra thực tế | Kết quả |
|---|---|---|---|---|---|---|
| TC-IMG-01 | Upload ảnh hợp lệ (JPG) | File avatar.jpg (500KB) | JPG/PNG, ≤ 2MB | Ảnh lưu vào `wwwroot/uploads/avatars/`. AvatarPath cập nhật. Hiển thị trên hồ sơ. | *(Ghi)* | ⏳ |
| TC-IMG-02 | Upload ảnh hợp lệ (PNG) | File avatar.png (1MB) | | Tương tự TC-IMG-01 | *(Ghi)* | ⏳ |
| TC-IMG-03 | Upload file sai định dạng | File document.pdf | Chỉ JPG/PNG | Lỗi: "Chỉ chấp nhận JPG/PNG và tối đa 2MB" | *(Ghi)* | ⏳ |
| TC-IMG-04 | Upload file quá 2MB | File bigimage.jpg (5MB) | Tối đa 2MB | Lỗi: "Chỉ chấp nhận JPG/PNG và tối đa 2MB" | *(Ghi)* | ⏳ |
| TC-IMG-05 | Upload không chọn file | Không chọn file, nhấn Upload | avatarFile = null | Lỗi: "Tệp ảnh không hợp lệ" | *(Ghi)* | ⏳ |
| TC-IMG-06 | SA/HR upload ảnh cho NV khác | Đăng nhập SA, upload ảnh cho NV id=2 | Role SA/HR | Ảnh cập nhật cho NV id=2. Redirect về Edit. | *(Ghi)* | ⏳ |

## 2. Upload tài liệu hồ sơ

| TC-ID | Mục tiêu test | Dữ liệu đầu vào | Ràng buộc | Đầu ra dự kiến | Đầu ra thực tế | Kết quả |
|---|---|---|---|---|---|---|
| TC-DOC-01 | Upload PDF thành công | File bangcap.pdf (2MB), Title: "Bằng ĐH", Category: "BangCap" | PDF/Word/JPG/PNG, ≤ 10MB | File lưu `uploads/documents/`. Bản ghi EmployeeDocument tạo trong DB. | *(Ghi)* | ⏳ |
| TC-DOC-02 | Upload Word thành công | File hopdong.docx (1MB) | | Tương tự TC-DOC-01 | *(Ghi)* | ⏳ |
| TC-DOC-03 | Upload file sai định dạng | File script.exe | Chỉ PDF/Word/JPG/PNG | Lỗi: "Chỉ chấp nhận PDF, Word, JPG, PNG và tối đa 10MB" | *(Ghi)* | ⏳ |
| TC-DOC-04 | Upload file quá 10MB | File bigdoc.pdf (15MB) | Tối đa 10MB | Lỗi tương tự TC-DOC-03 | *(Ghi)* | ⏳ |
| TC-DOC-05 | Xóa tài liệu | Nhấn Xóa trên tài liệu đã upload | Có quyền (NV xóa của mình, SA/HR xóa bất kỳ) | File vật lý bị xóa. Bản ghi DB bị xóa. Thông báo thành công. | *(Ghi)* | ⏳ |
| TC-DOC-06 | NV xóa tài liệu người khác | Employee cố xóa document thuộc NV khác | | Trả về 403 Forbidden | *(Ghi)* | ⏳ |
