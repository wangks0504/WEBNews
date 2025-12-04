namespace WEBNews.Models.Entities;

/// <summary>
/// 新闻接收人关联表 - 黄奕负责实现
/// </summary>
public class NewsReceiver
{
    /// <summary>
    /// 主键ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 新闻ID（外键）
    /// </summary>
    public int NewsId { get; set; }

    /// <summary>
    /// 接收人用户ID（外键）
    /// </summary>
    public int ReceiverId { get; set; }

    /// <summary>
    /// 指定时间
    /// </summary>
    public DateTime AssignedAt { get; set; } = DateTime.Now;

    /// <summary>
    /// 新闻（导航属性）
    /// </summary>
    public virtual News News { get; set; } = null!;

    /// <summary>
    /// 接收人（导航属性）
    /// </summary>
    public virtual User Receiver { get; set; } = null!;
}

