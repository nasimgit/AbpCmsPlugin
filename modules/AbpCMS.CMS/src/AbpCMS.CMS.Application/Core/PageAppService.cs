using AbpCMS.CMS.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Repositories;

namespace AbpCMS.CMS.Core;

public class PageAppService : ApplicationService, IPageAppService
{
    private readonly PageManager _pageManager;
    private readonly IRepository<Page, Guid> _repository;
    private readonly IDistributedCache<PageDto> _pageCache;

    public PageAppService(
        IRepository<Page, Guid> repository,
        PageManager pageManager,
        IDistributedCache<PageDto> pageCache)
    {
        _pageManager = pageManager;
        _repository = repository;
        _pageCache = pageCache;
    }

    public async Task<PageDto> GetBySlugAsync(string slug)
    {
        var page = await _pageManager.GetBySlugAsync(slug);
        return ObjectMapper.Map<Page, PageDto>(page);
    }

    [Authorize(CMSPermissions.Pages.Publish)]
    public async Task<PageDto> PublishAsync(Guid id)
    {
        var page = await _pageManager.PublishAsync(id);
        
        // Invalidate cache when page is published (content might have changed)
        await InvalidatePageCacheAsync(page.Slug);
        
        return ObjectMapper.Map<Page, PageDto>(page);
    }

    [Authorize(CMSPermissions.Pages.Publish)]
    public async Task<PageDto> UnpublishAsync(Guid id)
    {
        var page = await _pageManager.UnpublishAsync(id);
        
        // Invalidate cache when page is unpublished
        await InvalidatePageCacheAsync(page.Slug);
        
        return ObjectMapper.Map<Page, PageDto>(page);
    }

    public async Task<PageDto> GetAsync(Guid id)
    {
        var page = await _repository.GetAsync(id);
        return ObjectMapper.Map<Page, PageDto>(page);
    }

    public async Task<PagedResultDto<PageDto>> GetListAsync(GetPageListDto input)
    {
        var query = await _repository.GetQueryableAsync();

        if (!string.IsNullOrWhiteSpace(input.Filter))
        {
            query = query.Where(p => p.Title.Contains(input.Filter) || p.Content.Contains(input.Filter));
        }

        if (!string.IsNullOrWhiteSpace(input.Author))
        {
            query = query.Where(p => p.Author == input.Author);
        }

        if (!string.IsNullOrWhiteSpace(input.Tags))
        {
            query = query.Where(p => p.Tags.Contains(input.Tags));
        }

        if (input.IsMarkdown.HasValue)
        {
            query = query.Where(p => p.IsMarkdown == input.IsMarkdown.Value);
        }

        var totalCount = await AsyncExecuter.CountAsync(query);

        query = query.OrderBy(p => p.CreationTime);
        query = query.Skip(input.SkipCount).Take(input.MaxResultCount);

        var pages = await AsyncExecuter.ToListAsync(query);

        return new PagedResultDto<PageDto>(
            totalCount,
            ObjectMapper.Map<List<Page>, List<PageDto>>(pages)
        );
    }

    public async Task<PagedResultDto<PageDto>> GetPublishedPagesAsync(GetPageListDto input)
    {
        var query = await _repository.GetQueryableAsync();
        query = query.Where(p => p.IsPublished);

        if (!string.IsNullOrWhiteSpace(input.Filter))
        {
            query = query.Where(p => p.Title.Contains(input.Filter) || p.Content.Contains(input.Filter));
        }

        if (!string.IsNullOrWhiteSpace(input.Author))
        {
            query = query.Where(p => p.Author == input.Author);
        }

        if (!string.IsNullOrWhiteSpace(input.Tags))
        {
            query = query.Where(p => p.Tags.Contains(input.Tags));
        }

        if (input.IsMarkdown.HasValue)
        {
            query = query.Where(p => p.IsMarkdown == input.IsMarkdown.Value);
        }

        var totalCount = await AsyncExecuter.CountAsync(query);

        query = query.OrderBy(p => p.CreationTime);
        query = query.Skip(input.SkipCount).Take(input.MaxResultCount);

        var pages = await AsyncExecuter.ToListAsync(query);

        return new PagedResultDto<PageDto>(
            totalCount,
            ObjectMapper.Map<List<Page>, List<PageDto>>(pages)
        );
    }

    [Authorize(CMSPermissions.Pages.Create)]
    public async Task<PageDto> CreateAsync(CreatePageDto input)
    {
        var page = await _pageManager.CreateAsync(
            input.Title,
            input.Slug,
            input.Content,
            input.Author,
            input.IsMarkdown,
            input.MetaTitle,
            input.MetaDescription,
            input.MetaKeywords,
            input.SortOrder,
            input.Tags,
            input.IsHomePage
        );
        return ObjectMapper.Map<Page, PageDto>(page);
    }

    [Authorize(CMSPermissions.Pages.Edit)]
    public async Task<PageDto> UpdateAsync(Guid id, UpdatePageDto input)
    {
        // Get the page before updating to get the old slug for cache invalidation
        var existingPage = await _repository.GetAsync(id);
        var oldSlug = existingPage.Slug;
        
        var page = await _pageManager.UpdateAsync(
            id,
            input.Title,
            input.Slug,
            input.Content,
            input.Author,
            input.IsMarkdown,
            input.MetaTitle,
            input.MetaDescription,
            input.MetaKeywords,
            input.SortOrder,
            input.Tags,
            input.IsHomePage
        );
        
        // Invalidate cache for both old and new slug (in case slug changed)
        await InvalidatePageCacheAsync(oldSlug);
        if (oldSlug != input.Slug)
        {
            await InvalidatePageCacheAsync(input.Slug);
        }
        
        return ObjectMapper.Map<Page, PageDto>(page);
    }

    [Authorize(CMSPermissions.Pages.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        // Get the page before deleting to get the slug for cache invalidation
        var page = await _repository.GetAsync(id);
        var slug = page.Slug;
        
        await _repository.DeleteAsync(id);
        
        // Invalidate cache for the deleted page
        await InvalidatePageCacheAsync(slug);
    }

    public async Task<PageDto?> GetHomePageAsync()
    {
        var page = await _pageManager.GetHomePageAsync();
        return page != null ? ObjectMapper.Map<Page, PageDto>(page) : null;
    }

    [Authorize(CMSPermissions.Pages.Edit)]
    public async Task<PageDto> SetAsHomePageAsync(Guid id)
    {
        var page = await _pageManager.SetAsHomePageAsync(id);
        return ObjectMapper.Map<Page, PageDto>(page);
    }

    private async Task InvalidatePageCacheAsync(string slug)
    {
        await _pageCache.RemoveAsync(slug);
    }

}
