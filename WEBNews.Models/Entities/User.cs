namespace WEBNews.Models.Entities;

/// <summary>
/// 用户实体类 - 欧阳负责实现
/// </summary>
public class User
{
    /// <summary>
    /// 用户ID（主键）
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// 用户名（登录用）
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 密码（加密后存储）
    /// </summary>
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>
    /// 邮箱
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// 真实姓名
    /// </summary>
    public string? RealName { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    /// <summary>
    /// 该用户发布的新闻
    /// </summary>
    public virtual ICollection<News> News { get; set; } = new List<News>();

    /// <summary>
    /// 该用户接收的新闻
    /// </summary>
    public virtual ICollection<NewsReceiver> ReceivedNews { get; set; } = new List<NewsReceiver>();
}

