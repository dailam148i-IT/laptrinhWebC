# 5.4.6. Test Cases: Giao diện Responsive

## 1. Kiểm thử trên các thiết bị

| TC-ID | Tên test | Thiết bị / Kích thước | Trang test | Đầu ra dự kiến | Đầu ra thực tế | Kết quả |
|---|---|---|---|---|---|---|
| TC-UI-01 | Desktop Full HD | 1920x1080 | Tất cả | Sidebar hiển thị đầy đủ. Bảng không bị tràn. Layout 2 cột cho form. | *(Ghi)* | ⏳ |
| TC-UI-02 | Laptop | 1366x768 | Tất cả | Sidebar thu gọn icon. Content co lại vừa màn hình. | *(Ghi)* | ⏳ |
| TC-UI-03 | Tablet dọc | 768x1024 | Tất cả | Sidebar ẩn, có hamburger menu. Bảng có scroll ngang nếu cần. | *(Ghi)* | ⏳ |
| TC-UI-04 | Mobile | 375x667 | Tất cả | Sidebar ẩn. Form xếp dọc 1 cột. Bảng cuộn ngang. Nút không bị che. | *(Ghi)* | ⏳ |
| TC-UI-05 | Trang Login responsive | 360x640 | Login | Form đăng nhập căn giữa, không tràn hoặc bị che. | *(Ghi)* | ⏳ |
| TC-UI-06 | Chi tiết hồ sơ mobile | 375x667 | Details | Layout chuyển từ 2 cột sang 1 cột. Ảnh xếp trên, thông tin xếp dưới. | *(Ghi)* | ⏳ |
| TC-UI-07 | Dashboard mobile | 375x667 | Dashboard | 4 card thống kê xếp dọc 1 cột. Bảng có scroll ngang. | *(Ghi)* | ⏳ |

## 2. Kiểm thử trình duyệt

| TC-ID | Trình duyệt | Phiên bản | Đầu ra dự kiến | Đầu ra thực tế | Kết quả |
|---|---|---|---|---|---|
| TC-BR-01 | Google Chrome | Mới nhất | Tất cả chức năng hoạt động bình thường | *(Ghi)* | ⏳ |
| TC-BR-02 | Mozilla Firefox | Mới nhất | Tương tự Chrome | *(Ghi)* | ⏳ |
| TC-BR-03 | Microsoft Edge | Mới nhất | Tương tự Chrome | *(Ghi)* | ⏳ |

## 3. Tiêu chí hoàn thành kiểm thử

- [ ] Tất cả test cases ở mức **PASS** (đánh dấu ✅)
- [ ] Không còn lỗi nghiêm trọng (Critical / High severity)
- [ ] Giao diện hiển thị đúng trên ít nhất 3 kích thước màn hình
- [ ] Phân quyền chặt chẽ: không có trường hợp vượt quyền
- [ ] Audit Log ghi nhận đúng mọi thao tác thay đổi dữ liệu
