namespace WEBNews.Models.DTOs.Auth;

/// <summary>
/// 登录响应DTO - 欧阳使用
/// </summary>
public class LoginResponseDto
{
    /// <summary>
    /// JWT Token
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// Token过期时间
    /// </summary>
    public DateTime ExpireTime { get; set; }

    /// <summary>
    /// 用户ID
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName { get; set; } = string.Empty;
}

