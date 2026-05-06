# ĐẶC TẢ API RESTFUL
## Hệ thống Quản lý Sơ yếu lý lịch

---

## 1. Tổng quan
- **Base URL:** `/api`
- **Format:** JSON
- **Encoding:** UTF-8
- **Authentication:** Kiểm tra Session (hoặc Token nếu mở rộng)

## 2. Quy ước chung

### HTTP Status Codes
| Code | Ý nghĩa |
|------|---------|
| 200 | OK - Thành công |
| 201 | Created - Tạo mới thành công |
| 400 | Bad Request - Dữ liệu không hợp lệ |
| 401 | Unauthorized - Chưa đăng nhập |
| 403 | Forbidden - Không có quyền |
| 404 | Not Found - Không tìm thấy |
| 500 | Internal Server Error - Lỗi hệ thống |

### Response Format
```json
// Thành công (danh sách)
{
    "success": true,
    "data": [...],
    "totalCount": 100,
    "page": 1,
    "pageSize": 10,
    "totalPages": 10
}

// Thành công (chi tiết)
{
    "success": true,
    "data": {...}
}

// Lỗi
{
    "success": false,
    "message": "Mô tả lỗi",
    "errors": ["Chi tiết lỗi 1", "Chi tiết lỗi 2"]
}
```

---

## 3. Endpoints chi tiết

### 3.1 Resumes API

#### GET /api/resumes
Lấy danh sách hồ sơ sơ yếu lý lịch (có phân trang và tìm kiếm).

**Query Parameters:**
| Tham số | Kiểu | Bắt buộc | Mặc định | Mô tả |
|---------|------|----------|----------|-------|
| page | int | Không | 1 | Trang hiện tại |
| pageSize | int | Không | 10 | Số bản ghi mỗi trang |
| search | string | Không | "" | Tìm theo họ tên, CCCD, email |
| departmentId | int | Không | null | Lọc theo phòng ban |

**Response (200):**
```json
{
    "success": true,
    "data": [
        {
            "resumeId": 1,
            "fullName": "Nguyễn Văn A",
            "dateOfBirth": "1995-05-15",
            "gender": "Nam",
            "identityNumber": "012345678901",
            "phoneNumber": "0901234567",
            "email": "nguyenvana@email.com",
            "departmentName": "Phòng Kỹ thuật",
            "positionName": "Nhân viên",
            "avatarPath": "/Uploads/Avatars/1_20260505.jpg"
        }
    ],
    "totalCount": 50,
    "page": 1,
    "pageSize": 10,
    "totalPages": 5
}
```

---

#### GET /api/resumes/{id}
Lấy chi tiết một hồ sơ sơ yếu lý lịch.

**Path Parameters:**
| Tham số | Kiểu | Mô tả |
|---------|------|-------|
| id | int | Mã hồ sơ (ResumeId) |

**Response (200):**
```json
{
    "success": true,
    "data": {
        "resumeId": 1,
        "fullName": "Nguyễn Văn A",
        "dateOfBirth": "1995-05-15",
        "gender": "Nam",
        "identityNumber": "012345678901",
        "birthPlace": "Hà Nội",
        "homeTown": "Hà Nội",
        "ethnicity": "Kinh",
        "religion": "Không",
        "permanentAddress": "123 Đường ABC, Quận XYZ, Hà Nội",
        "temporaryAddress": null,
        "phoneNumber": "0901234567",
        "email": "nguyenvana@email.com",
        "avatarPath": "/Uploads/Avatars/1_20260505.jpg",
        "maritalStatus": "Độc thân",
        "departmentName": "Phòng Kỹ thuật",
        "positionName": "Nhân viên",
        "educations": [
            {
                "educationId": 1,
                "schoolName": "Đại học Bách Khoa Hà Nội",
                "major": "Công nghệ thông tin",
                "degree": "Đại học",
                "startYear": 2013,
                "endYear": 2017,
                "classification": "Giỏi"
            }
        ],
        "workExperiences": [
            {
                "experienceId": 1,
                "companyName": "Công ty ABC",
                "jobTitle": "Lập trình viên",
                "startDate": "2017-09-01",
                "endDate": "2020-12-31",
                "description": "<p>Phát triển ứng dụng web...</p>"
            }
        ],
        "skills": [
            {
                "skillId": 1,
                "skillName": "C# .NET",
                "skillType": "Kỹ năng",
                "proficiency": "Thành thạo",
                "issuedDate": null,
                "issuedBy": null
            }
        ],
        "familyMembers": [
            {
                "familyMemberId": 1,
                "fullName": "Nguyễn Văn B",
                "relationship": "Bố",
                "birthYear": 1965,
                "occupation": "Giáo viên",
                "address": "Hà Nội"
            }
        ]
    }
}
```

---

#### POST /api/resumes
Tạo hồ sơ sơ yếu lý lịch mới.

**Request Body:**
```json
{
    "userId": 2,
    "fullName": "Trần Thị B",
    "dateOfBirth": "1998-03-20",
    "gender": "Nữ",
    "identityNumber": "098765432109",
    "birthPlace": "TP.HCM",
    "permanentAddress": "456 Đường DEF, Quận 1, TP.HCM",
    "phoneNumber": "0912345678",
    "email": "tranthib@email.com",
    "departmentId": 1,
    "positionId": 4
}
```

**Response (201):**
```json
{
    "success": true,
    "data": {
        "resumeId": 2,
        "message": "Tạo hồ sơ thành công"
    }
}
```

---

#### PUT /api/resumes/{id}
Cập nhật hồ sơ sơ yếu lý lịch.

**Request Body:** Tương tự POST, gửi các trường cần cập nhật.

**Response (200):**
```json
{
    "success": true,
    "data": {
        "resumeId": 2,
        "message": "Cập nhật hồ sơ thành công"
    }
}
```

---

#### DELETE /api/resumes/{id}
Xóa hồ sơ sơ yếu lý lịch (cascade xóa các bản ghi liên quan).

**Response (200):**
```json
{
    "success": true,
    "message": "Xóa hồ sơ thành công"
}
```

---

### 3.2 Departments API

#### GET /api/departments
Lấy danh sách phòng ban.

**Response (200):**
```json
{
    "success": true,
    "data": [
        { "departmentId": 1, "departmentName": "Phòng Nhân sự", "description": "Quản lý tuyển dụng" },
        { "departmentId": 2, "departmentName": "Phòng Kỹ thuật", "description": "Phát triển sản phẩm" }
    ]
}
```

---

### 3.3 Positions API

#### GET /api/positions
Lấy danh sách chức vụ.

**Response (200):**
```json
{
    "success": true,
    "data": [
        { "positionId": 1, "positionName": "Giám đốc", "description": "Giám đốc điều hành" },
        { "positionId": 2, "positionName": "Trưởng phòng", "description": "Quản lý phòng ban" }
    ]
}
```
