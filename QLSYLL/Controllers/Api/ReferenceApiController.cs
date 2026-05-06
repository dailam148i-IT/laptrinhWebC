using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLSYLL.Infrastructure.Auth;
using QLSYLL.Models;

namespace QLSYLL.Controllers.Api;

[ApiController]
[ApiKeyAuthorize]
public class ReferenceApiController(ApplicationDbContext dbContext) : ControllerBase
{
    [HttpGet("api/departments")]
    public async Task<IActionResult> Departments()
    {
        var data = await dbContext.Departments.Where(x => !x.IsDeleted).Select(x => new { x.Id, x.Name, x.Code }).ToListAsync();
        return Ok(new { success = true, data, message = (string?)null });
    }

    [HttpGet("api/departments/{id:int}")]
    public async Task<IActionResult> Department(int id)
    {
        var data = await dbContext.Departments.Where(x => x.Id == id && !x.IsDeleted).Select(x => new { x.Id, x.Name, x.Code, x.Description }).FirstOrDefaultAsync();
        return data is null ? NotFound(new { success = false, message = "Không tìm thấy phòng ban." }) : Ok(new { success = true, data });
    }

    [HttpGet("api/positions")]
    public async Task<IActionResult> Positions()
    {
        var data = await dbContext.Positions.Where(x => !x.IsDeleted).Select(x => new { x.Id, x.Name, x.Level, x.BaseSalary }).ToListAsync();
        return Ok(new { success = true, data, message = (string?)null });
    }

    [HttpGet("api/users")]
    public async Task<IActionResult> Users()
    {
        var data = await dbContext.Users
            .Where(x => !x.IsDeleted)
            .Include(x => x.Role)
            .Select(x => new { x.Id, x.Username, x.FullName, x.Email, Role = x.Role.Code, x.IsActive })
            .ToListAsync();
        return Ok(new { success = true, data, message = (string?)null });
    }
}
