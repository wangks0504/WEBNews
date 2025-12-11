using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEBNews.Data;

namespace WEBNews.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AttachmentsController : ControllerBase
{
    private readonly AppDbContext _db;

    public AttachmentsController(AppDbContext db)
    {
        _db = db;
    }

    // GET /api/attachments/{id}/download
    [HttpGet("{id:int}/download")]
    public async Task<IActionResult> Download(int id)
    {
        var att = await _db.Attachments.FirstOrDefaultAsync(a => a.AttachmentId == id);
        if (att == null || !System.IO.File.Exists(att.FilePath))
            return NotFound();

        var stream = System.IO.File.OpenRead(att.FilePath);
        return File(stream, att.FileType ?? "application/octet-stream", att.FileName);
    }
}
