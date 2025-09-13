using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.MultiTenancy;

namespace AbpCMS.CMS.Core;

public class PageManager : DomainService
{
    private readonly IRepository<Page, Guid> _pageRepository;
    private readonly IRepository<PageCategory, Guid> _categoryRepository;
    private readonly IRepository<PageCategoryMapping, Guid> _mappingRepository;

    public PageManager(
        IRepository<Page, Guid> pageRepository,
        IRepository<PageCategory, Guid> categoryRepository,
        IRepository<PageCategoryMapping, Guid> mappingRepository)
    {
        _pageRepository = pageRepository;
        _categoryRepository = categoryRepository;
        _mappingRepository = mappingRepository;
    }

    public async Task<Page> CreateAsync(
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
        bool isHomePage = false)
    {
        await CheckSlugUniquenessAsync(slug);

        var page = new Page(
            GuidGenerator.Create(),
            title,
            slug,
            content,
            author,
            isMarkdown,
            metaTitle,
            metaDescription,
            metaKeywords,
            sortOrder,
            tags,
            CurrentTenant.Id
        );

        if (isHomePage)
        {
            await UnsetCurrentHomePageAsync();
            page.SetAsHomePage();
        }

        return await _pageRepository.InsertAsync(page);
    }

    public async Task<Page> UpdateAsync(
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
        bool isHomePage = false)
    {
        var page = await _pageRepository.GetAsync(id);

        if (page.Slug != slug)
        {
            await CheckSlugUniquenessAsync(slug, id);
        }

        page.SetTitle(title);
        page.SetSlug(slug);
        page.SetContent(content);
        page.SetAuthor(author);
        page.UpdateContent(content, isMarkdown);
        page.SetMetaTitle(metaTitle);
        page.SetMetaDescription(metaDescription);
        page.SetMetaKeywords(metaKeywords);
        page.SetSortOrder(sortOrder);
        page.SetTags(tags);

        if (isHomePage && !page.IsHomePage)
        {
            await UnsetCurrentHomePageAsync();
            page.SetAsHomePage();
        }
        else if (!isHomePage && page.IsHomePage)
        {
            page.UnsetAsHomePage();
        }

        return await _pageRepository.UpdateAsync(page);
    }

    public async Task<Page> PublishAsync(Guid id)
    {
        var page = await _pageRepository.GetAsync(id);
        page.Publish();
        return await _pageRepository.UpdateAsync(page);
    }

    public async Task<Page> UnpublishAsync(Guid id)
    {
        var page = await _pageRepository.GetAsync(id);
        page.Unpublish();
        return await _pageRepository.UpdateAsync(page);
    }

    public async Task<Page?> GetBySlugAsync(string slug)
    {
        return await _pageRepository.FirstOrDefaultAsync(p => p.Slug == slug && p.IsPublished);
    }

    public async Task<Page?> GetHomePageAsync()
    {
        return await _pageRepository.FirstOrDefaultAsync(p => p.IsHomePage && p.IsPublished);
    }

    public async Task<Page> SetAsHomePageAsync(Guid id)
    {
        var page = await _pageRepository.GetAsync(id);
        await UnsetCurrentHomePageAsync();
        page.SetAsHomePage();
        return await _pageRepository.UpdateAsync(page);
    }

    private async Task UnsetCurrentHomePageAsync()
    {
        var currentHomePage = await _pageRepository.FirstOrDefaultAsync(p => p.IsHomePage);
        if (currentHomePage != null)
        {
            currentHomePage.UnsetAsHomePage();
            await _pageRepository.UpdateAsync(currentHomePage);
        }
    }

    public async Task<PageCategory> CreateCategoryAsync(
        string name,
        string slug,
        string? description = null,
        string? color = null,
        string? icon = null,
        int sortOrder = 0,
        Guid? parentId = null)
    {
        await CheckCategorySlugUniquenessAsync(slug);

        var category = new PageCategory(
            GuidGenerator.Create(),
            name,
            slug,
            description,
            color,
            icon,
            sortOrder,
            parentId,
            CurrentTenant.Id
        );

        return await _categoryRepository.InsertAsync(category);
    }

    public async Task<PageCategory> UpdateCategoryAsync(
        Guid id,
        string name,
        string slug,
        string? description = null,
        string? color = null,
        string? icon = null,
        int sortOrder = 0,
        Guid? parentId = null)
    {
        var category = await _categoryRepository.GetAsync(id);

        if (category.Slug != slug)
        {
            await CheckCategorySlugUniquenessAsync(slug, id);
        }

        category.SetName(name);
        category.SetSlug(slug);
        category.SetDescription(description);
        category.SetColor(color);
        category.SetIcon(icon);
        category.SetSortOrder(sortOrder);
        category.SetParent(parentId);

        return await _categoryRepository.UpdateAsync(category);
    }

    public async Task AddPageToCategoryAsync(Guid pageId, Guid categoryId)
    {
        var existingMapping = await _mappingRepository.FirstOrDefaultAsync(
            m => m.PageId == pageId && m.CategoryId == categoryId);

        if (existingMapping == null)
        {
            var mapping = new PageCategoryMapping(
                GuidGenerator.Create(),
                pageId,
                categoryId
            );
            await _mappingRepository.InsertAsync(mapping);
        }
    }

    public async Task RemovePageFromCategoryAsync(Guid pageId, Guid categoryId)
    {
        var mapping = await _mappingRepository.FirstOrDefaultAsync(
            m => m.PageId == pageId && m.CategoryId == categoryId);

        if (mapping != null)
        {
            await _mappingRepository.DeleteAsync(mapping);
        }
    }

    private async Task CheckSlugUniquenessAsync(string slug, Guid? excludeId = null)
    {
        var queryable = await _pageRepository.GetQueryableAsync();
        var query = queryable.Where(p => p.Slug == slug);
        
        if (excludeId.HasValue)
        {
            query = query.Where(p => p.Id != excludeId.Value);
        }

        if (CurrentTenant.Id.HasValue)
        {
            query = query.Where(p => p.TenantId == CurrentTenant.Id);
        }

        if (await AsyncExecuter.AnyAsync(query))
        {
            throw new BusinessException(CMSErrorCodes.SlugAlreadyExists)
                .WithData("Slug", slug);
        }
    }

    private async Task CheckCategorySlugUniquenessAsync(string slug, Guid? excludeId = null)
    {
        var queryable = await _categoryRepository.GetQueryableAsync();
        var query = queryable.Where(c => c.Slug == slug);
        
        if (excludeId.HasValue)
        {
            query = query.Where(c => c.Id != excludeId.Value);
        }

        if (CurrentTenant.Id.HasValue)
        {
            query = query.Where(c => c.TenantId == CurrentTenant.Id);
        }

        if (await AsyncExecuter.AnyAsync(query))
        {
            throw new BusinessException(CMSErrorCodes.CategorySlugAlreadyExists)
                .WithData("Slug", slug);
        }
    }
}
