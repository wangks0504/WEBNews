namespace WEBNews.Models.DTOs.News;

/// <summary>
/// 新闻DTO - 心怡使用
/// </summary>
public class NewsDto
{
    public int NewsId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? Summary { get; set; }
    public string Author { get; set; } = string.Empty;
    public DateTime PublishTime { get; set; }
}

