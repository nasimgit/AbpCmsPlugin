using System;
using Volo.Abp.Application.Dtos;

namespace AbpCMS.CMS.Core;

public class PageDto : FullAuditedEntityDto<Guid>
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? MetaKeywords { get; set; }
    public bool IsPublished { get; set; }
    public bool IsMarkdown { get; set; }
    public bool IsHomePage { get; set; }
    public DateTime? PublishedAt { get; set; }
    public int SortOrder { get; set; }
    public string? Tags { get; set; }
    public string? Author { get; set; }
}
