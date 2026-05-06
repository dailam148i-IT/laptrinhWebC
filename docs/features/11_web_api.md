# Chức năng 11: Web API RESTful

> **Controller:** `Api/ResumesApiController`, `Api/DepartmentsApiController`... (mới)  
> **Actor:** 🔐 SA only

---

## Mô tả

Cung cấp API RESTful để hệ thống bên ngoài có thể truy vấn dữ liệu hồ sơ nhân sự. Chỉ Super Admin mới có quyền sử dụng (qua API Key hoặc Token).

---

## Phân quyền

| Hành động | 🔐 SA | 🏢 HR | 👔 MG | 👤 EM |
|-----------|:------:|:------:|:------:|:------:|
| Truy cập API | ✅ | ❌ | ❌ | ❌ |

---

## 11.1 Danh sách Endpoints

### Resumes API
| Method | Endpoint | Mô tả |
|--------|----------|-------|
| GET | `/api/resumes` | Lấy danh sách hồ sơ (hỗ trợ filter, paging) |
| GET | `/api/resumes/{id}` | Lấy chi tiết 1 hồ sơ |
| POST | `/api/resumes` | Tạo hồ sơ mới |
| PUT | `/api/resumes/{id}` | Cập nhật hồ sơ |
| DELETE | `/api/resumes/{id}` | Xóa hồ sơ |

### Departments API
| Method | Endpoint | Mô tả |
|--------|----------|-------|
| GET | `/api/departments` | Lấy danh sách phòng ban |
| GET | `/api/departments/{id}` | Lấy chi tiết phòng ban |

### Positions API
| Method | Endpoint | Mô tả |
|--------|----------|-------|
| GET | `/api/positions` | Lấy danh sách chức vụ |

### Users API
| Method | Endpoint | Mô tả |
|--------|----------|-------|
| GET | `/api/users` | Lấy danh sách tài khoản |

---

## 11.2 Xác thực API

### Phương án
- **API Key** trong header: `X-API-Key: {key}` (đơn giản, phù hợp hệ thống nội bộ).
- Hoặc **JWT Token** (phức tạp hơn, bảo mật hơn).

### Yêu cầu
- [ ] Tạo middleware xác thực API Key / JWT
- [ ] Chỉ cho phép request có key/token hợp lệ
- [ ] Ghi AuditLog cho mỗi API call

---

## 11.3 Response Format

```json
{
  "success": true,
  "data": { ... },
  "message": null,
  "pagination": {
    "page": 1,
    "pageSize": 20,
    "totalItems": 156,
    "totalPages": 8
  }
}
```

---

### Trạng thái code
❌ Chưa có bất kỳ API Controller nào.
