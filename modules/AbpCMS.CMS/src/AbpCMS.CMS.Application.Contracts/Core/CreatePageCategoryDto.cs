using System;
using System.ComponentModel.DataAnnotations;

namespace AbpCMS.CMS.Core;

public class CreatePageCategoryDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [Required]
    [StringLength(100)]
    public string Slug { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    [StringLength(20)]
    public string? Color { get; set; }

    [StringLength(50)]
    public string? Icon { get; set; }

    public int SortOrder { get; set; }

    public Guid? ParentId { get; set; }
}
