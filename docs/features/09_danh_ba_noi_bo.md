# Chức năng 9: Danh bạ nội bộ

> **Controller:** `ContactController` (mới)  
> **View:** `Index.cshtml` (mới)  
> **Actor:** 🔐 SA, 🏢 HR, 👔 MG, 👤 EM (tất cả, dữ liệu hiển thị khác nhau)

---

## Mô tả

Trang danh bạ cho phép tất cả nhân viên xem thông tin liên lạc cơ bản của đồng nghiệp. Thông tin nhạy cảm (CCCD, ngày sinh, lương, gia đình) được **ẩn hoàn toàn** với MG và EM.

---

## Phân quyền theo Actor

| Thông tin hiển thị | 🔐 SA | 🏢 HR | 👔 MG | 👤 EM |
|--------------------|:------:|:------:|:------:|:------:|
| Họ và tên | ✅ | ✅ | ✅ | ✅ |
| Email | ✅ | ✅ | ✅ | ✅ |
| Số điện thoại | ✅ | ✅ | ✅ | ✅ |
| Phòng ban | ✅ | ✅ | ✅ | ✅ |
| Chức vụ | ✅ | ✅ | ✅ | ✅ |
| Ảnh đại diện | ✅ | ✅ | ✅ | ✅ |
| Ngày sinh | ✅ | ✅ | ❌ | ❌ |
| CCCD/CMND | ✅ | ✅ | ❌ | ❌ |
| Địa chỉ | ✅ | ✅ | ❌ | ❌ |
| Lương | ✅ | ✅ | ❌ | ❌ |
| Thông tin gia đình | ✅ | ✅ | ❌ | ❌ |

---

## 9.1 Danh sách danh bạ (Index)

### Route
- **GET** `/Contact`

### Giao diện
- Hiển thị dạng **card grid** hoặc **bảng** (có thể chuyển đổi).
- Hỗ trợ **tìm kiếm** theo tên, phòng ban.
- Hỗ trợ **lọc** theo phòng ban.

### Logic
1. Kiểm tra Role từ Session.
2. Query `Resumes` JOIN `Departments`, `Positions`.
3. Chỉ SELECT các cột được phép theo Role.
4. SA/HR: hiển thị đầy đủ. MG/EM: chỉ hiển thị thông tin công khai.

### Yêu cầu DB
- [ ] Tạo `ContactController`
- [ ] Query chỉ SELECT cột cần thiết theo Role (không gửi dữ liệu nhạy cảm về client)
- [ ] Tìm kiếm + lọc + phân trang
- [ ] Tạo View dạng card/bảng

---

### Trạng thái code
❌ Chưa có. Cần tạo mới Controller + View.
