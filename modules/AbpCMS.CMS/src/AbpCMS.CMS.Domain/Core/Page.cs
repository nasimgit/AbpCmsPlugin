using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace AbpCMS.CMS.Core;

public class Page : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public string Title { get; private set; } = string.Empty;
    public string Slug { get; private set; } = string.Empty;
    public string Content { get; private set; } = string.Empty;
    public string? MetaTitle { get; private set; }
    public string? MetaDescription { get; private set; }
    public string? MetaKeywords { get; private set; }
    public bool IsPublished { get; private set; }
    public bool IsMarkdown { get; private set; }
    public bool IsHomePage { get; private set; }
    public DateTime? PublishedAt { get; private set; }
    public Guid? TenantId { get; set; }
    public int SortOrder { get; private set; }
    public string? Tags { get; private set; }
    public string? Author { get; private set; }

    private Page()
    {
        // For ORM
    }

    public Page(
        Guid id,
        string title,
        string slug,
        string content,
        string? author,
        bool isMarkdown = false,
        string? metaTitle = null,
        string? metaDescription = null,
        string? metaKeywords = null,
        int sortOrder = 0,
        string? tags = null,
        Guid? tenantId = null) : base(id)
    {
        SetTitle(title);
        SetSlug(slug);
        SetContent(content);
        SetAuthor(author);
        IsMarkdown = isMarkdown;
        MetaTitle = metaTitle;
        MetaDescription = metaDescription;
        MetaKeywords = metaKeywords;
        SortOrder = sortOrder;
        Tags = tags;
        TenantId = tenantId;
        IsPublished = false;
        IsHomePage = false;
        PublishedAt = null;
    }

    public void SetTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Title cannot be null or empty", nameof(title));
        }
        Title = title.Trim();
    }

    public void SetSlug(string slug)
    {
        if (string.IsNullOrWhiteSpace(slug))
        {
            throw new ArgumentException("Slug cannot be null or empty", nameof(slug));
        }
        
        // Normalize slug - remove special characters and convert to lowercase
        var normalizedSlug = slug.ToLowerInvariant()
            .Replace(" ", "-")
            .Replace("_", "-");
        
        // Remove multiple consecutive dashes
        while (normalizedSlug.Contains("--"))
        {
            normalizedSlug = normalizedSlug.Replace("--", "-");
        }
        
        // Remove leading/trailing dashes
        normalizedSlug = normalizedSlug.Trim('-');
        
        if (string.IsNullOrEmpty(normalizedSlug))
        {
            throw new ArgumentException("Invalid slug format", nameof(slug));
        }
        
        Slug = normalizedSlug;
    }

    public void SetContent(string content)
    {
        if (content == null)
        {
            throw new ArgumentException("Content cannot be null", nameof(content));
        }
        
        if (content.Length > 8000)
        {
            throw new ArgumentException("Content cannot exceed 8,000 characters", nameof(content));
        }
        
        Content = content;
    }

    public void SetAuthor(string? author)
    {
        Author = author?.Trim();
    }

    public void SetMetaTitle(string? metaTitle)
    {
        MetaTitle = metaTitle?.Trim();
    }

    public void SetMetaDescription(string? metaDescription)
    {
        MetaDescription = metaDescription?.Trim();
    }

    public void SetMetaKeywords(string? metaKeywords)
    {
        MetaKeywords = metaKeywords?.Trim();
    }

    public void SetSortOrder(int sortOrder)
    {
        SortOrder = sortOrder;
    }

    public void SetTags(string? tags)
    {
        Tags = tags?.Trim();
    }

    public void Publish()
    {
        if (IsPublished)
        {
            return;
        }

        IsPublished = true;
        PublishedAt = DateTime.UtcNow;
    }

    public void Unpublish()
    {
        if (!IsPublished)
        {
            return;
        }

        IsPublished = false;
        PublishedAt = null;
    }

    public void UpdateContent(string content, bool isMarkdown = false)
    {
        SetContent(content);
        IsMarkdown = isMarkdown;
    }

    public void UpdateSeo(string? metaTitle, string? metaDescription, string? metaKeywords)
    {
        SetMetaTitle(metaTitle);
        SetMetaDescription(metaDescription);
        SetMetaKeywords(metaKeywords);
    }

    public void SetAsHomePage()
    {
        IsHomePage = true;
    }

    public void UnsetAsHomePage()
    {
        IsHomePage = false;
    }
}
