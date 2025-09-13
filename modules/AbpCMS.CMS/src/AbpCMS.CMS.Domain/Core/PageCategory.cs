using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace AbpCMS.CMS.Core;

public class PageCategory : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public string Name { get; private set; } = string.Empty;
    public string Slug { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public string? Color { get; private set; }
    public string? Icon { get; private set; }
    public int SortOrder { get; private set; }
    public bool IsActive { get; private set; }
    public Guid? ParentId { get; private set; }
    public Guid? TenantId { get; set; }

    private PageCategory()
    {
        // For ORM
    }

    public PageCategory(
        Guid id,
        string name,
        string slug,
        string? description = null,
        string? color = null,
        string? icon = null,
        int sortOrder = 0,
        Guid? parentId = null,
        Guid? tenantId = null) : base(id)
    {
        SetName(name);
        SetSlug(slug);
        Description = description?.Trim();
        Color = color?.Trim();
        Icon = icon?.Trim();
        SortOrder = sortOrder;
        ParentId = parentId;
        TenantId = tenantId;
        IsActive = true;
    }

    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be null or empty", nameof(name));
        }
        Name = name.Trim();
    }

    public void SetSlug(string slug)
    {
        if (string.IsNullOrWhiteSpace(slug))
        {
            throw new ArgumentException("Slug cannot be null or empty", nameof(slug));
        }
        
        // Normalize slug
        var normalizedSlug = slug.ToLowerInvariant()
            .Replace(" ", "-")
            .Replace("_", "-");
        
        while (normalizedSlug.Contains("--"))
        {
            normalizedSlug = normalizedSlug.Replace("--", "-");
        }
        
        normalizedSlug = normalizedSlug.Trim('-');
        
        if (string.IsNullOrEmpty(normalizedSlug))
        {
            throw new ArgumentException("Invalid slug format", nameof(slug));
        }
        
        Slug = normalizedSlug;
    }

    public void SetDescription(string? description)
    {
        Description = description?.Trim();
    }

    public void SetColor(string? color)
    {
        Color = color?.Trim();
    }

    public void SetIcon(string? icon)
    {
        Icon = icon?.Trim();
    }

    public void SetSortOrder(int sortOrder)
    {
        SortOrder = sortOrder;
    }

    public void SetParent(Guid? parentId)
    {
        ParentId = parentId;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}
