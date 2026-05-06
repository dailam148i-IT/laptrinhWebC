# MÔ TẢ GIAO DIỆN NGƯỜI DÙNG (UI Specification)
## Hệ thống Quản lý Sơ yếu lý lịch

---

## 1. Sitemap (Bản đồ trang)

```
Hệ thống QLSYLL
│
├── [PUBLIC] Trang Đăng nhập (/Account/Login)
│
├── [ADMIN] Khu vực Quản trị
│   ├── Dashboard (/Admin/Dashboard)
│   ├── Quản lý Hồ sơ SYLL
│   │   ├── Danh sách hồ sơ (/Resume/Index)
│   │   ├── Thêm hồ sơ (/Resume/Create)
│   │   ├── Chi tiết hồ sơ (/Resume/Details/{id})
│   │   └── Sửa hồ sơ (/Resume/Edit/{id})
│   ├── Quản lý Tài khoản (/User/Index)
│   ├── Quản lý Phòng ban (/Department/Index)
│   ├── Quản lý Chức vụ (/Position/Index)
│   ├── Quản lý Thông báo (/Announcement/Index)
│   └── Đổi mật khẩu (/Account/ChangePassword)
│
└── [EMPLOYEE] Khu vực Nhân viên
    ├── Trang cá nhân (/Employee/Dashboard)
    ├── Xem/Sửa hồ sơ cá nhân (/Employee/MyResume)
    ├── Danh bạ nhân viên (/Employee/Directory)
    ├── Xem thông báo (/Employee/Announcements)
    └── Đổi mật khẩu (/Account/ChangePassword)
```

---

## 2. Mô tả chi tiết từng trang

### 2.1 Trang Đăng nhập (/Account/Login)
```
┌──────────────────────────────────────────────────────┐
│                                                      │
│              ┌──────────────────────┐                │
│              │    LOGO HỆ THỐNG     │                │
│              │  Quản lý Sơ yếu      │                │
│              │     lý lịch          │                │
│              └──────────────────────┘                │
│                                                      │
│              ┌──────────────────────┐                │
│              │ 👤 Tên đăng nhập     │                │
│              │ [__________________ ]│                │
│              │                      │                │
│              │ 🔒 Mật khẩu          │                │
│              │ [__________________ ]│                │
│              │                      │                │
│              │ ☐ Nhớ tài khoản      │                │
│              │                      │                │
│              │ [   ĐĂNG NHẬP    ]   │                │
│              └──────────────────────┘                │
│                                                      │
└──────────────────────────────────────────────────────┘

Ghi chú:
- Validation: Bắt buộc nhập Username và Password
- "Nhớ tài khoản" → sử dụng Cookies
- Nền gradient hoặc hình ảnh chuyên nghiệp
- Form căn giữa, responsive
```

### 2.2 Layout Admin (Sidebar + Header)
```
┌──────────────────────────────────────────────────────────────┐
│ HEADER BAR                              Xin chào, Admin ▼   │
│ ☰  HỆ THỐNG QLSYLL                     [Đăng xuất]         │
├──────────────┬───────────────────────────────────────────────┤
│  SIDEBAR     │                                               │
│              │   CONTENT AREA                                │
│  📊 Dashboard│                                               │
│              │   (Nội dung thay đổi theo trang)              │
│  📋 Hồ sơ   │                                               │
│    SYLL      │                                               │
│              │                                               │
│  👤 Tài     │                                               │
│    khoản     │                                               │
│              │                                               │
│  🏢 Phòng   │                                               │
│    ban       │                                               │
│              │                                               │
│  💼 Chức vụ  │                                               │
│              │                                               │
│  📢 Thông   │                                               │
│    báo       │                                               │
│              │                                               │
│  🔑 Đổi MK  │                                               │
│              │                                               │
├──────────────┴───────────────────────────────────────────────┤
│ FOOTER: © 2026 Hệ thống QLSYLL - Nhóm XX                   │
└──────────────────────────────────────────────────────────────┘

Responsive:
- Desktop: Sidebar cố định bên trái
- Tablet/Mobile: Sidebar ẩn, hamburger menu (☰) để mở
```

### 2.3 Trang Dashboard Admin (/Admin/Dashboard)
```
┌─────────────────────────────────────────────────────────────┐
│                                                             │
│  ┌──────────┐  ┌──────────┐  ┌──────────┐  ┌──────────┐   │
│  │ 👥 120   │  │ 🏢 5     │  │ 💼 8     │  │ 📢 3     │   │
│  │ Nhân viên│  │ Phòng ban│  │ Chức vụ  │  │ Thông báo│   │
│  └──────────┘  └──────────┘  └──────────┘  └──────────┘   │
│                                                             │
│  ┌─────────────────────────────────────────────────────┐   │
│  │ NHÂN VIÊN MỚI THÊM GẦN ĐÂY                        │   │
│  │ ┌─────┬───────────┬────────────┬──────────────────┐ │   │
│  │ │ STT │ Họ và tên │ Phòng ban  │ Ngày tạo         │ │   │
│  │ ├─────┼───────────┼────────────┼──────────────────┤ │   │
│  │ │ 1   │ Nguyễn A  │ Kỹ thuật   │ 05/05/2026       │ │   │
│  │ │ 2   │ Trần B    │ Nhân sự    │ 04/05/2026       │ │   │
│  │ └─────┴───────────┴────────────┴──────────────────┘ │   │
│  └─────────────────────────────────────────────────────┘   │
│                                                             │
└─────────────────────────────────────────────────────────────┘
```

### 2.4 Danh sách Hồ sơ SYLL (/Resume/Index)
```
┌─────────────────────────────────────────────────────────────┐
│  DANH SÁCH SƠ YẾU LÝ LỊCH                [+ Thêm mới]     │
│                                                             │
│  🔍 Tìm kiếm: [_______________] Phòng ban: [Tất cả ▼]     │
│                                                             │
│  ┌─────┬──────┬───────────┬───────┬────────┬──────────────┐ │
│  │ STT │ Ảnh  │ Họ và tên │ CCCD  │Phòng   │ Thao tác     │ │
│  ├─────┼──────┼───────────┼───────┼────────┼──────────────┤ │
│  │ 1   │ [📷] │ Nguyễn A  │ 012...│Kỹ thuật│ 👁 ✏️ 🗑️    │ │
│  │ 2   │ [📷] │ Trần B    │ 098...│Nhân sự │ 👁 ✏️ 🗑️    │ │
│  │ 3   │ [📷] │ Lê C      │ 034...│Kế toán │ 👁 ✏️ 🗑️    │ │
│  │ ... │      │           │       │        │              │ │
│  └─────┴──────┴───────────┴───────┴────────┴──────────────┘ │
│                                                             │
│  Hiển thị 1-10 / 50 bản ghi    [<] [1] [2] [3] [4] [5] [>]│
│                                                             │
└─────────────────────────────────────────────────────────────┘

Ghi chú:
- Tìm kiếm và lọc bằng AJAX (không reload trang)
- Phân trang hiển thị tối đa 10 bản ghi
- 👁 = Xem chi tiết, ✏️ = Sửa, 🗑️ = Xóa (confirm trước khi xóa)
```

### 2.5 Form Thêm/Sửa Hồ sơ (/Resume/Create, /Resume/Edit/{id})
```
┌─────────────────────────────────────────────────────────────┐
│  THÊM MỚI / CẬP NHẬT HỒ SƠ SƠ YẾU LÝ LỊCH               │
│                                                             │
│  ── THÔNG TIN CÁ NHÂN ──────────────────────────────────   │
│                                                             │
│  ┌─────────────────┐  Họ và tên *: [__________________ ]   │
│  │                 │  Ngày sinh *: [____/____/_________ ]   │
│  │   [UPLOAD ẢNH]  │  Giới tính *: ○ Nam  ○ Nữ  ○ Khác    │
│  │                 │  CCCD/CMND *: [__________________ ]   │
│  └─────────────────┘  Email *:     [__________________ ]   │
│                       SĐT *:      [__________________ ]   │
│                                                             │
│  Nơi sinh:        [__________________________________ ]    │
│  Quê quán:        [__________________________________ ]    │
│  Dân tộc:         [_______ ▼]    Tôn giáo: [_______ ▼]    │
│  Hôn nhân:        [_______ ▼]                              │
│  Địa chỉ TT *:   [__________________________________ ]    │
│  Địa chỉ tạm trú:[__________________________________ ]    │
│  Phòng ban:       [_______ ▼]    Chức vụ:  [_______ ▼]    │
│                                                             │
│  ── QUÁ TRÌNH HỌC VẤN ─────────────── [+ Thêm] ────────   │
│  ┌────────────┬────────────┬────────┬───────┬──────┬─────┐  │
│  │ Trường     │ Chuyên ngành│ Trình độ│ Từ   │ Đến  │ Xóa │  │
│  ├────────────┼────────────┼────────┼───────┼──────┼─────┤  │
│  │ [________] │ [________] │ [__ ▼] │ [___] │ [___]│ 🗑️ │  │
│  └────────────┴────────────┴────────┴───────┴──────┴─────┘  │
│                                                             │
│  ── KINH NGHIỆM LÀM VIỆC ──────────── [+ Thêm] ────────   │
│  ┌────────────┬──────────┬────────┬───────┬──────────────┐  │
│  │ Công ty    │ Chức vụ  │ Từ ngày│Đến ngày│ Mô tả       │  │
│  ├────────────┼──────────┼────────┼───────┼──────────────┤  │
│  │ [________] │ [______] │ [____] │ [____]│ [CKEditor  ] │  │
│  └────────────┴──────────┴────────┴───────┴──────────────┘  │
│                                                             │
│  ── KỸ NĂNG & CHỨNG CHỈ ────────────── [+ Thêm] ────────   │
│  (Bảng tương tự)                                            │
│                                                             │
│  ── THÔNG TIN GIA ĐÌNH ─────────────── [+ Thêm] ────────   │
│  (Bảng tương tự)                                            │
│                                                             │
│           [  LƯU HỒ SƠ  ]    [  HỦY BỎ  ]                 │
│                                                             │
└─────────────────────────────────────────────────────────────┘

Ghi chú:
- Các trường có dấu * là bắt buộc (Validation)
- Upload ảnh hỗ trợ preview trước khi lưu
- CKEditor được tích hợp vào ô "Mô tả" công việc
- [+ Thêm] cho phép thêm nhiều dòng động (dynamic rows) bằng JS
```

### 2.6 Chi tiết Hồ sơ (/Resume/Details/{id})
```
┌─────────────────────────────────────────────────────────────┐
│  CHI TIẾT SƠ YẾU LÝ LỊCH               [✏️ Sửa] [← Quay lại]│
│                                                             │
│  ┌──────────┐  ┌──────────────────────────────────────────┐ │
│  │          │  │ HỌ VÀ TÊN: NGUYỄN VĂN A                │ │
│  │  [ẢNH    │  │ Ngày sinh: 15/05/1995  |  Giới tính: Nam│ │
│  │   ĐẠI    │  │ CCCD: 012345678901                      │ │
│  │  DIỆN]   │  │ Email: nguyenvana@email.com              │ │
│  │          │  │ SĐT: 0901234567                         │ │
│  └──────────┘  │ Phòng ban: Kỹ thuật | Chức vụ: Nhân viên│ │
│                └──────────────────────────────────────────┘ │
│                                                             │
│  ── Thông tin khác ───────────────────────────────────────   │
│  Nơi sinh: Hà Nội | Quê quán: Hà Nội                       │
│  Dân tộc: Kinh | Tôn giáo: Không                           │
│  Địa chỉ thường trú: 123 Đường ABC, Quận XYZ, Hà Nội      │
│                                                             │
│  ── Quá trình học vấn ───────────────────────────────────   │
│  ┌────────────────────────┬──────────────┬──────┬────────┐  │
│  │ Trường                 │ Chuyên ngành │ TĐ   │ Năm    │  │
│  ├────────────────────────┼──────────────┼──────┼────────┤  │
│  │ ĐH Bách Khoa Hà Nội   │ CNTT         │ ĐH   │ 13-17  │  │
│  └────────────────────────┴──────────────┴──────┴────────┘  │
│                                                             │
│  ── Kinh nghiệm làm việc ───────────────────────────────   │
│  (Bảng hiển thị)                                            │
│                                                             │
│  ── Kỹ năng & Chứng chỉ ────────────────────────────────   │
│  (Bảng hiển thị)                                            │
│                                                             │
│  ── Thông tin gia đình ──────────────────────────────────   │
│  (Bảng hiển thị)                                            │
│                                                             │
└─────────────────────────────────────────────────────────────┘
```

---

## 3. Bảng màu (Color Palette) — Xem chi tiết tại [DESIGN.md](./DESIGN.md)
| Mục đích | Màu | Code |
|----------|-----|------|
| Primary (Accent chính) | Xanh dương | `#2563eb` |
| Primary Hover | Xanh đậm hơn | `#1d4ed8` |
| Primary Light | Xanh nhạt (tint) | `#eff6ff` |
| Success | Xanh lá | `#16a34a` |
| Warning | Vàng cam | `#d97706` |
| Danger/Error | Đỏ | `#dc2626` |
| Sidebar BG | **Trắng** | `#ffffff` |
| Sidebar Active BG | Xanh nhạt | `#eff6ff` |
| Content BG (Canvas) | Xám cực nhạt | `#f8fafc` |
| Card / Table BG | **Trắng** | `#ffffff` |
| Header BG | **Trắng** | `#ffffff` |
| Text (Ink) | Xám đen | `#1e293b` |
| Text (Body) | Xám trung | `#475569` |
| Text (Muted) | Xám nhạt | `#64748b` |
| Border (Hairline) | Xám viền | `#e2e8f0` |

---

## 4. Breakpoints (Responsive)
| Thiết bị | Breakpoint | Ghi chú |
|----------|-----------|---------|
| Mobile | < 576px | 1 cột, sidebar ẩn |
| Tablet (dọc) | 576px - 767px | 1-2 cột |
| Tablet (ngang) | 768px - 991px | 2 cột, sidebar thu gọn |
| Desktop | 992px - 1199px | Sidebar + Content |
| Desktop lớn | ≥ 1200px | Full layout |
