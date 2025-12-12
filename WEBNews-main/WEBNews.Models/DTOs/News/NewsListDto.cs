namespace WEBNews.Models.DTOs.News;

/// <summary>
/// 新闻列表项DTO - 心怡使用
/// </summary>
public class NewsListDto
{
    public int NewsId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Summary { get; set; }
    public string Author { get; set; } = string.Empty;
    public DateTime PublishTime { get; set; }
}

