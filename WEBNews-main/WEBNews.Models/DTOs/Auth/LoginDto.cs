using System.ComponentModel.DataAnnotations;

namespace WEBNews.Models.DTOs.Auth;

/// <summary>
/// 用户登录DTO - 欧阳使用
/// </summary>
public class LoginDto
{
    /// <summary>
    /// 用户名
    /// </summary>
    [Required(ErrorMessage = "用户名不能为空")]
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 密码
    /// </summary>
    [Required(ErrorMessage = "密码不能为空")]
    public string Password { get; set; } = string.Empty;
}

