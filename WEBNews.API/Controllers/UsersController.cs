using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEBNews.Data;
using WEBNews.Models.DTOs;
using WEBNews.Models.DTOs.News;
using WEBNews.Models.DTOs.Users;

namespace WEBNews.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _db;

    public UsersController(AppDbContext db)
    {
        _db = db;
    }

    // GET /api/users
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<UserListItemDto>>>> GetUsers()
    {
        var list = await _db.Users
            .Select(u => new UserListItemDto { UserId = u.UserId, UserName = u.UserName, RealName = u.RealName })
            .ToListAsync();
        return Ok(ApiResponse<IEnumerable<UserListItemDto>>.Ok(list));
    }

    // GET /api/Users (messages - for current user)
    [HttpGet("messages")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<IEnumerable<NewsListDto>>>> GetMyMessages()
    {
        var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
        var list = await _db.NewsReceivers
            .Include(nr => nr.News)
            .ThenInclude(n => n.Author)
            .Where(nr => nr.ReceiverId == userId)
            .OrderByDescending(nr => nr.News.PublishTime)
            .Select(nr => new NewsListDto { NewsId = nr.News.NewsId, Title = nr.News.Title, Summary = nr.News.Summary, Author = nr.News.Author.UserName, PublishTime = nr.News.PublishTime })
            .ToListAsync();
        return Ok(ApiResponse<IEnumerable<NewsListDto>>.Ok(list));
    }
}
