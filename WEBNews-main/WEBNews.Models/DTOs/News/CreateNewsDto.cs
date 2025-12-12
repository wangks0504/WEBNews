using System.ComponentModel.DataAnnotations;

namespace WEBNews.Models.DTOs.News;

/// <summary>
/// 创建新闻DTO - 心怡使用
/// </summary>
public class CreateNewsDto
{
    /// <summary>
    /// 新闻标题
    /// </summary>
    [Required(ErrorMessage = "标题不能为空")]
    [StringLength(200, ErrorMessage = "标题最多200个字符")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 新闻内容
    /// </summary>
    [Required(ErrorMessage = "内容不能为空")]
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 新闻摘要
    /// </summary>
    [StringLength(500, ErrorMessage = "摘要最多500个字符")]
    public string? Summary { get; set; }
}

