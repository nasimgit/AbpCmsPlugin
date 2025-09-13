using System;
using Volo.Abp.Application.Dtos;

namespace AbpCMS.CMS.Core;

public class GetPageCategoryListDto : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public bool? IsActive { get; set; }
    public Guid? ParentId { get; set; }
}
