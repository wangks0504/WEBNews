using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WEBNews.Data;
using WEBNews.Models.DTOs;
using WEBNews.Models.DTOs.Auth;
using WEBNews.Models.Entities;

namespace WEBNews.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _config;

    public AuthController(AppDbContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<string>>> Register([FromBody] RegisterDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<string>.Fail(400, "参数错误"));

        var exists = await _db.Users.AnyAsync(u => u.UserName == dto.UserName);
        if (exists)
            return Conflict(ApiResponse<string>.Fail(409, "用户名已存在"));

        var user = new User
        {
            UserName = dto.UserName,
            Email = dto.Email,
            RealName = dto.RealName,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return Ok(ApiResponse<string>.Ok("注册成功"));
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<LoginResponseDto>>> Login([FromBody] LoginDto dto)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.UserName == dto.UserName);
        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            return Unauthorized(ApiResponse<LoginResponseDto>.Fail(401, "用户名或密码错误"));

        var jwtSection = _config.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expire = DateTime.UtcNow.AddMinutes(int.Parse(jwtSection["ExpireMinutes"]!));

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new(ClaimTypes.Name, user.UserName)
        };
        var token = new JwtSecurityToken(
            issuer: jwtSection["Issuer"],
            audience: jwtSection["Audience"],
            claims: claims,
            expires: expire,
            signingCredentials: creds
        );

        var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);

        var resp = new LoginResponseDto
        {
            Token = tokenStr,
            ExpireTime = expire,
            UserId = user.UserId,
            UserName = user.UserName
        };
        return Ok(ApiResponse<LoginResponseDto>.Ok(resp));
    }
}
