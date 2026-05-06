# BÁO CÁO BÀI TẬP LỚN
## Hệ thống Quản lý Sơ yếu lý lịch nhân sự (QLSYLL)

---

## Cấu trúc thư mục báo cáo

```
baocao/
├── README.md                          ← File này (Mục lục tổng)
├── 01_tong_quan/                      ← Chương 5.1 - Tổng quan về đề tài
│   ├── 01_mo_ta_he_thong.md           ← Mô tả hệ thống, nghiệp vụ cơ bản
│   ├── 02_khao_sat_bieu_mau.md        ← Khảo sát thực tế, biểu mẫu thu thập
│   └── 03_phan_tich_yeu_cau.md        ← Phân tích yêu cầu, chức năng theo vai trò
├── 02_phan_tich_thiet_ke/             ← Chương 5.2 - Phân tích và thiết kế
│   ├── 01_use_case_diagram.md         ← Biểu đồ Use-case (Mermaid)
│   ├── 02_thiet_ke_csdl.md            ← ERD, mô tả dữ liệu, ràng buộc
│   └── 03_sitemap_wireframe.md        ← Sitemap và wireframe mô tả
├── 03_thiet_ke_he_thong/              ← Chương 5.3 - Thiết kế hệ thống
│   ├── 01_kien_truc_tong_quan.md      ← Kiến trúc hệ thống, công nghệ
│   ├── 02_chuc_nang_xac_thuc.md       ← Đăng nhập, phân quyền, đổi MK
│   ├── 03_chuc_nang_dashboard.md      ← Dashboard theo vai trò
│   ├── 04_chuc_nang_ho_so.md          ← CRUD Hồ sơ SYLL
│   ├── 05_chuc_nang_phong_ban.md      ← Quản lý Phòng ban & Chức vụ
│   ├── 06_chuc_nang_tai_khoan.md      ← Quản lý Tài khoản
│   ├── 07_chuc_nang_thong_bao.md      ← Quản lý Thông báo
│   ├── 08_chuc_nang_danh_ba.md        ← Danh bạ nội bộ
│   └── 09_chuc_nang_audit_log.md      ← Audit Log
├── 04_kiem_thu/                        ← Chương 5.4 - Kiểm thử
│   ├── 01_muc_tieu_kiem_thu.md        ← Mục tiêu và phương pháp
│   ├── 02_test_case_xac_thuc.md       ← Test cases: Đăng nhập, phân quyền
│   ├── 03_test_case_ho_so.md          ← Test cases: CRUD Hồ sơ
│   ├── 04_test_case_upload.md         ← Test cases: Upload ảnh, tài liệu
│   ├── 05_test_case_phan_quyen.md     ← Test cases: Phân quyền RBAC
│   └── 06_test_case_giao_dien.md      ← Test cases: Responsive, UI
└── 05_ket_luan/                        ← Chương 5.5 & 5.6 - Kết luận
    ├── 01_ket_luan.md                 ← Kết luận tổng hợp
    ├── 02_phan_cong_cong_viec.md      ← Bảng phân công công việc
    └── 03_tai_lieu_tham_khao.md       ← Tài liệu tham khảo
```

> **Lưu ý:** Tất cả sơ đồ Mermaid trong báo cáo đều có thể render trực tiếp bằng công cụ hỗ trợ Markdown (VS Code, GitHub, hoặc plugin Mermaid). Khi in báo cáo Word/PDF, cần export diagram thành hình ảnh trước.
