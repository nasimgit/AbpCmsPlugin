using System;
using System.ComponentModel.DataAnnotations;

namespace AbpCMS.CMS.Core;

public class CreatePageDto
{
    [Required]
    [StringLength(200)]
    public string Title { get; set; }

    [Required]
    [StringLength(200)]
    public string Slug { get; set; }

    [Required]
    [StringLength(8000, ErrorMessage = "Content cannot exceed 8,000 characters")]
    public string Content { get; set; }

    [StringLength(200)]
    public string? MetaTitle { get; set; }

    [StringLength(500)]
    public string? MetaDescription { get; set; }

    [StringLength(200)]
    public string? MetaKeywords { get; set; }

    public bool IsMarkdown { get; set; }

    public bool IsHomePage { get; set; }

    public int SortOrder { get; set; }

    [StringLength(500)]
    public string? Tags { get; set; }

    [StringLength(100)]
    public string? Author { get; set; }
}
