# 5.3.8. Chức năng Danh bạ nội bộ

**Controller:** `ContactController.cs` | **Quyền:** Tất cả 4 vai trò

## 1. Mô tả
Cho phép nhân viên tra cứu thông tin liên lạc của đồng nghiệp trong toàn công ty.

## 2. Dữ liệu hiển thị

| # | Trường | SA/HR | Manager/Employee |
|---|---|:---:|:---:|
| 1 | Họ tên | ✅ | ✅ |
| 2 | Email | ✅ | ✅ |
| 3 | SĐT | ✅ | ✅ |
| 4 | Phòng ban | ✅ | ✅ |
| 5 | Chức vụ | ✅ | ✅ |
| 6 | Ngày sinh | ✅ | ✅ |
| 7 | CCCD | ✅ | ❌ (ẩn) |
| 8 | Địa chỉ | ✅ | ❌ (ẩn) |
| 9 | Lương | ✅ | ❌ (ẩn) |
| 10 | Thông tin gia đình | ✅ | ❌ (ẩn) |

**Logic phân quyền:** Biến `ShowSensitive = true/false` dựa trên role SA/HR.

## 3. Bộ lọc
- Tìm kiếm theo Họ tên hoặc Phòng ban
- Lọc theo Phòng ban (dropdown)

> **📸 Hình ảnh:** *(Chèn screenshot danh bạ nội bộ)*
