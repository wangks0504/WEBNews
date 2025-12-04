namespace WEBNews.Models.Entities;

/// <summary>
/// 附件实体类 - 黄奕负责实现
/// </summary>
public class Attachment
{
    /// <summary>
    /// 附件ID（主键）
    /// </summary>
    public int AttachmentId { get; set; }

    /// <summary>
    /// 文件名
    /// </summary>
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// 文件路径（服务器存储路径）
    /// </summary>
    public string FilePath { get; set; } = string.Empty;

    /// <summary>
    /// 文件大小（字节）
    /// </summary>
    public long FileSize { get; set; }

    /// <summary>
    /// 文件类型（MIME类型）
    /// </summary>
    public string? FileType { get; set; }

    /// <summary>
    /// 所属新闻ID（外键）
    /// </summary>
    public int NewsId { get; set; }

    /// <summary>
    /// 上传时间
    /// </summary>
    public DateTime UploadTime { get; set; } = DateTime.Now;

    /// <summary>
    /// 所属新闻（导航属性）
    /// </summary>
    public virtual News News { get; set; } = null!;
}

