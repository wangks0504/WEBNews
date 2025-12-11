using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEBNews.Data;
using WEBNews.Models.DTOs;
using WEBNews.Models.DTOs.News;
using WEBNews.Models.Entities;
using System.Security.Claims;

namespace WEBNews.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NewsController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IWebHostEnvironment _env;

    public NewsController(AppDbContext db, IWebHostEnvironment env)
    {
        _db = db;
        _env = env;
    }

    // GET /api/news
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<NewsListDto>>>> GetNewsList()
    {
        var list = await _db.News
            .Include(n => n.Author)
            .OrderByDescending(n => n.PublishTime)
            .Select(n => new NewsListDto
            {
                NewsId = n.NewsId,
                Title = n.Title,
                Summary = n.Summary,
                Author = n.Author.UserName,
                PublishTime = n.PublishTime
            }).ToListAsync();
        return Ok(ApiResponse<IEnumerable<NewsListDto>>.Ok(list));
    }

    // GET /api/news/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<NewsDto>>> GetNewsById(int id)
    {
        var n = await _db.News.Include(x => x.Author).FirstOrDefaultAsync(x => x.NewsId == id);
        if (n == null) return NotFound(ApiResponse<NewsDto>.Fail(404, "未找到新闻"));
        return Ok(ApiResponse<NewsDto>.Ok(new NewsDto
        {
            NewsId = n.NewsId,
            Title = n.Title,
            Content = n.Content,
            Summary = n.Summary,
            Author = n.Author.UserName,
            PublishTime = n.PublishTime
        }));
    }

    // POST /api/news (auth)
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<ApiResponse<NewsDto>>> Create([FromBody] CreateNewsDto dto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var news = new News
        {
            Title = dto.Title,
            Content = dto.Content,
            Summary = dto.Summary,
            AuthorId = userId,
            PublishTime = DateTime.Now
        };
        _db.News.Add(news);
        await _db.SaveChangesAsync();
        var author = await _db.Users.FindAsync(userId);
        return Ok(ApiResponse<NewsDto>.Ok(new NewsDto
        {
            NewsId = news.NewsId,
            Title = news.Title,
            Content = news.Content,
            Summary = news.Summary,
            Author = author?.UserName ?? string.Empty,
            PublishTime = news.PublishTime
        }));
    }

    // POST /api/news/{id}/receivers (auth)
    [HttpPost("{id:int}/receivers")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<string>>> SetReceivers(int id, [FromBody] int[] userIds)
    {
        var news = await _db.News.FirstOrDefaultAsync(n => n.NewsId == id);
        if (news == null) return NotFound(ApiResponse<string>.Fail(404, "新闻不存在"));

        if (userIds == null || userIds.Length == 0)
        {
            return BadRequest(ApiResponse<string>.Fail(400, "请至少选择一个接收人"));
        }

        var existingUsers = await _db.Users.Where(u => userIds.Contains(u.UserId)).Select(u => u.UserId).ToListAsync();
        var notFoundUsers = userIds.Except(existingUsers).ToArray();
        if (notFoundUsers.Length > 0)
        {
            return NotFound(ApiResponse<string>.Fail(404, $"以下用户不存在: {string.Join(",", notFoundUsers)}"));
        }

        var exist = await _db.NewsReceivers.Where(x => x.NewsId == id).ToListAsync();
        _db.NewsReceivers.RemoveRange(exist);

        foreach (var uid in existingUsers)
        {
            _db.NewsReceivers.Add(new NewsReceiver
            {
                NewsId = id,
                ReceiverId = uid,
                AssignedAt = DateTime.Now
            });
        }
        await _db.SaveChangesAsync();
        return Ok(ApiResponse<string>.Ok("接收人已更新"));
    }

    // POST /api/news/{id}/attachments (auth, multipart)
    [HttpPost("{id:int}/attachments")]
    [Authorize]
    [RequestSizeLimit(104857600)] // 100MB
    [Consumes("multipart/form-data")] // For Swagger
    public async Task<ActionResult<ApiResponse<string>>> UploadAttachment(int id, IFormFile file)
    {
        var news = await _db.News.FirstOrDefaultAsync(n => n.NewsId == id);
        if (news == null) return NotFound(ApiResponse<string>.Fail(404, "未找到新闻"));
        if (file == null || file.Length == 0) return BadRequest(ApiResponse<string>.Fail(400, "文件无效"));

        var attachDir = Path.Combine(_env.ContentRootPath, "App_Data", "attachments");
        Directory.CreateDirectory(attachDir);
        var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
        var fullPath = Path.Combine(attachDir, fileName);
        await using (var stream = System.IO.File.Create(fullPath))
        {
            await file.CopyToAsync(stream);
        }

        var att = new Attachment
        {
            FileName = file.FileName,
            FilePath = fullPath,
            FileSize = file.Length,
            FileType = file.ContentType,
            NewsId = id,
            UploadTime = DateTime.Now
        };
        _db.Attachments.Add(att);
        await _db.SaveChangesAsync();

        return Ok(ApiResponse<string>.Ok("上传成功"));
    }
}
