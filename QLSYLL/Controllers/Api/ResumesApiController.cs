using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLSYLL.Infrastructure.Auth;
using QLSYLL.Models;

namespace QLSYLL.Controllers.Api;

[ApiController]
[Route("api/resumes")]
[ApiKeyAuthorize]
public class ResumesApiController(ApplicationDbContext dbContext) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(int page = 1, int pageSize = 20)
    {
        var query = dbContext.Employees.Where(x => !x.IsDeleted).Include(x => x.Department).Include(x => x.Position);
        var totalItems = await query.CountAsync();
        var data = await query.Skip((page - 1) * pageSize).Take(pageSize)
            .Select(x => new
            {
                x.Id,
                x.FullName,
                x.Phone,
                Department = x.Department.Name,
                Position = x.Position.Name,
                x.Status
            })
            .ToListAsync();

        return Ok(ApiResponse(data, page, pageSize, totalItems));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var data = await dbContext.Employees.Where(x => x.Id == id && !x.IsDeleted)
            .Include(x => x.Department)
            .Include(x => x.Position)
            .Select(x => new
            {
                x.Id,
                x.FullName,
                x.Phone,
                x.PersonalEmail,
                Address = x.CurrentAddress,
                Department = x.Department.Name,
                Position = x.Position.Name,
                x.Status
            })
            .FirstOrDefaultAsync();

        return data is null ? NotFound(ApiResponse<object?>(null, message: "Không tìm thấy hồ sơ.")) : Ok(ApiResponse(data));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Employee resume)
    {
        dbContext.Employees.Add(resume);
        await dbContext.SaveChangesAsync();
        return Ok(ApiResponse(resume, message: "Tạo hồ sơ thành công."));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Employee request)
    {
        var resume = await dbContext.Employees.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        if (resume is null) return NotFound(ApiResponse<object?>(null, message: "Không tìm thấy hồ sơ."));

        resume.FullName = request.FullName;
        resume.Phone = request.Phone;
        resume.PersonalEmail = request.PersonalEmail;
        resume.CurrentAddress = request.CurrentAddress;
        resume.DepartmentId = request.DepartmentId;
        resume.PositionId = request.PositionId;
        resume.Status = request.Status;
        resume.UpdatedAt = DateTime.UtcNow;
        await dbContext.SaveChangesAsync();

        return Ok(ApiResponse(resume, message: "Cập nhật hồ sơ thành công."));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var resume = await dbContext.Employees.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        if (resume is null) return NotFound(ApiResponse<object?>(null, message: "Không tìm thấy hồ sơ."));

        resume.IsDeleted = true;
        await dbContext.SaveChangesAsync();
        return Ok(ApiResponse<object?>(null, message: "Xóa hồ sơ thành công."));
    }

    private static object ApiResponse<T>(T data, int page = 1, int pageSize = 20, int totalItems = 0, string? message = null)
    {
        var totalPages = totalItems == 0 ? 0 : (int)Math.Ceiling(totalItems / (double)pageSize);
        return new
        {
            success = true,
            data,
            message,
            pagination = new { page, pageSize, totalItems, totalPages }
        };
    }
}
