namespace WEBNews.Models.Entities;

/// <summary>
/// 新闻实体类 - 心怡负责实现
/// </summary>
public class News
{
    /// <summary>
    /// 新闻ID（主键）
    /// </summary>
    public int NewsId { get; set; }

    /// <summary>
    /// 新闻标题
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 新闻内容
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 新闻摘要（可选）
    /// </summary>
    public string? Summary { get; set; }

    /// <summary>
    /// 作者ID（外键）
    /// </summary>
    public int AuthorId { get; set; }

    /// <summary>
    /// 发布时间
    /// </summary>
    public DateTime PublishTime { get; set; } = DateTime.Now;

    /// <summary>
    /// 作者信息（导航属性）
    /// </summary>
    public virtual User Author { get; set; } = null!;

    /// <summary>
    /// 新闻的附件列表
    /// </summary>
    public virtual ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();

    /// <summary>
    /// 新闻的接收人列表
    /// </summary>
    public virtual ICollection<NewsReceiver> Receivers { get; set; } = new List<NewsReceiver>();
}

