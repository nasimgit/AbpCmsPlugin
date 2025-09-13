using System;
using Volo.Abp.Application.Dtos;

namespace AbpCMS.CMS.Core;

public class GetPageListDto : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public bool? IsPublished { get; set; }
    public bool? IsMarkdown { get; set; }
    public string? Author { get; set; }
    public string? Tags { get; set; }
}
