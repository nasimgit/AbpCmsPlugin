using System;
using Volo.Abp.Domain.Entities;

namespace AbpCMS.CMS.Core;

public class PageCategoryMapping : Entity<Guid>
{
    public Guid PageId { get; set; }
    public Guid CategoryId { get; set; }

    private PageCategoryMapping()
    {
        // For ORM
    }

    public PageCategoryMapping(Guid id, Guid pageId, Guid categoryId) : base(id)
    {
        PageId = pageId;
        CategoryId = categoryId;
    }
}
