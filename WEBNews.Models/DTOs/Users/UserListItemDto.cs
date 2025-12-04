namespace WEBNews.Models.DTOs.Users;

/// <summary>
/// 用户列表项 DTO
/// </summary>
public class UserListItemDto
{
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string? RealName { get; set; }
}
