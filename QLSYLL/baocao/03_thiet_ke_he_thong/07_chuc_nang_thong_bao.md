# 5.3.7. Chức năng Thông báo nội bộ

**Controller:** `AnnouncementController.cs`

## 1. Danh sách thông báo (`/Announcement/Index`)
- **Quyền xem:** Tất cả 4 vai trò
- Hiển thị: Tiêu đề, Mức ưu tiên (Bình thường/Quan trọng/Khẩn cấp), Trạng thái (Bản nháp/Đã đăng), Tác giả, Ngày tạo
- Phân trang, tìm kiếm theo tiêu đề

> **📸 Hình ảnh:** *(Chèn screenshot danh sách thông báo)*

## 2. Đăng thông báo (`/Announcement/Create`)
- **Quyền:** SA, HR
- Trường: Tiêu đề, Nội dung, Mức ưu tiên (dropdown), Trạng thái (Bản nháp / Đã đăng)
- Nội dung hỗ trợ HTML (Rich Text)

> **📸 Hình ảnh:** *(Chèn screenshot form đăng thông báo)*

## 3. Sửa/Xóa thông báo
- **Quyền:** SA, HR
- Sửa: cập nhật Title, Content, Priority, Status
- Xóa: Hard Delete khỏi database
