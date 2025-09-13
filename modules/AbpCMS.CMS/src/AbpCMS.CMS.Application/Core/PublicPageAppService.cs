using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Caching;

namespace AbpCMS.CMS.Core;

[AllowAnonymous]
public class PublicPageAppService : ApplicationService, IPublicPageAppService
{
    private readonly IRepository<Page, Guid> _pageRepository;
    private readonly IRepository<PageCategory, Guid> _categoryRepository;
    private readonly IDistributedCache<PageDto> _pageCache;

    public PublicPageAppService(
        IRepository<Page, Guid> pageRepository,
        IRepository<PageCategory, Guid> categoryRepository,
        IDistributedCache<PageDto> pageCache)
    {
        _pageRepository = pageRepository;
        _categoryRepository = categoryRepository;
        _pageCache = pageCache;
    }

    public async Task<PageDto> GetBySlugAsync(string slug)
    {
        return await _pageCache.GetOrAddAsync(
            slug, //Cache key
            async () => await GetPageFromDatabaseAsync(slug) ?? throw new UserFriendlyException($"Page with slug '{slug}' not found or yet not page is published."),
            () => new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddHours(24)
            }
        ) ?? throw new UserFriendlyException($"Page with slug '{slug}' not found or yet not page is published.");
    }

    private async Task<PageDto?> GetPageFromDatabaseAsync(string slug)
    {
        var page = await _pageRepository.FirstOrDefaultAsync(p => p.Slug == slug && p.IsPublished);
        
        if (page == null)
        {
            return null;
        }

        return ObjectMapper.Map<Page, PageDto>(page);
    }

    public async Task<PagedResultDto<PageDto>> GetPublishedPagesAsync(GetPageListDto input)
    {
        var query = await _pageRepository.GetQueryableAsync();
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

        query = ApplySorting(query, input);
        query = ApplyPaging(query, input);

        var pages = await AsyncExecuter.ToListAsync(query);

        return new PagedResultDto<PageDto>(
            totalCount,
            ObjectMapper.Map<List<Page>, List<PageDto>>(pages)
        );
    }

    public async Task<PagedResultDto<PageCategoryDto>> GetActiveCategoriesAsync(GetPageCategoryListDto input)
    {
        var query = await _categoryRepository.GetQueryableAsync();
        query = query.Where(c => c.IsActive);

        if (!string.IsNullOrWhiteSpace(input.Filter))
        {
            query = query.Where(c => c.Name.Contains(input.Filter) || c.Description.Contains(input.Filter));
        }

        if (input.ParentId.HasValue)
        {
            query = query.Where(c => c.ParentId == input.ParentId.Value);
        }

        var totalCount = await AsyncExecuter.CountAsync(query);

        query = ApplySorting(query, input);
        query = ApplyPaging(query, input);

        var categories = await AsyncExecuter.ToListAsync(query);

        return new PagedResultDto<PageCategoryDto>(
            totalCount,
            ObjectMapper.Map<List<PageCategory>, List<PageCategoryDto>>(categories)
        );
    }

    public async Task<PageCategoryDto> GetCategoryBySlugAsync(string slug)
    {
        var category = await _categoryRepository.FirstOrDefaultAsync(c => c.Slug == slug && c.IsActive);
        
        if (category == null)
        {
            throw new UserFriendlyException($"Category with slug '{slug}' not found.");
        }

        return ObjectMapper.Map<PageCategory, PageCategoryDto>(category);
    }

    public async Task<PageDto?> GetHomePageAsync()
    {
        var page = await _pageRepository.FirstOrDefaultAsync(p => p.IsHomePage && p.IsPublished);
        return page != null ? ObjectMapper.Map<Page, PageDto>(page) : null;
    }

    private IQueryable<T> ApplySorting<T>(IQueryable<T> query, PagedAndSortedResultRequestDto input)
    {
        if (!string.IsNullOrWhiteSpace(input.Sorting))
        {
            // Simple sorting implementation - in real scenario, you might want to use a more sophisticated approach
            if (typeof(T) == typeof(Page))
            {
                var pageQuery = query as IQueryable<Page>;
                switch (input.Sorting.ToLowerInvariant())
                {
                    case "title":
                        pageQuery = pageQuery.OrderBy(p => p.Title);
                        break;
                    case "title desc":
                        pageQuery = pageQuery.OrderByDescending(p => p.Title);
                        break;
                    case "createdtime":
                        pageQuery = pageQuery.OrderBy(p => p.CreationTime);
                        break;
                    case "createdtime desc":
                        pageQuery = pageQuery.OrderByDescending(p => p.CreationTime);
                        break;
                    case "sortorder":
                        pageQuery = pageQuery.OrderBy(p => p.SortOrder);
                        break;
                    case "sortorder desc":
                        pageQuery = pageQuery.OrderByDescending(p => p.SortOrder);
                        break;
                    default:
                        pageQuery = pageQuery.OrderBy(p => p.SortOrder).ThenBy(p => p.CreationTime);
                        break;
                }
                return pageQuery as IQueryable<T>;
            }
            else if (typeof(T) == typeof(PageCategory))
            {
                var categoryQuery = query as IQueryable<PageCategory>;
                switch (input.Sorting.ToLowerInvariant())
                {
                    case "name":
                        categoryQuery = categoryQuery.OrderBy(c => c.Name);
                        break;
                    case "name desc":
                        categoryQuery = categoryQuery.OrderByDescending(c => c.Name);
                        break;
                    case "sortorder":
                        categoryQuery = categoryQuery.OrderBy(c => c.SortOrder);
                        break;
                    case "sortorder desc":
                        categoryQuery = categoryQuery.OrderByDescending(c => c.SortOrder);
                        break;
                    default:
                        categoryQuery = categoryQuery.OrderBy(c => c.SortOrder).ThenBy(c => c.Name);
                        break;
                }
                return categoryQuery as IQueryable<T>;
            }
        }
        else
        {
            // Default sorting
            if (typeof(T) == typeof(Page))
            {
                var pageQuery = query as IQueryable<Page>;
                pageQuery = pageQuery.OrderBy(p => p.SortOrder).ThenBy(p => p.CreationTime);
                return pageQuery as IQueryable<T>;
            }
            else if (typeof(T) == typeof(PageCategory))
            {
                var categoryQuery = query as IQueryable<PageCategory>;
                categoryQuery = categoryQuery.OrderBy(c => c.SortOrder).ThenBy(c => c.Name);
                return categoryQuery as IQueryable<T>;
            }
        }

        return query;
    }

    private IQueryable<T> ApplyPaging<T>(IQueryable<T> query, PagedAndSortedResultRequestDto input)
    {
        return query.Skip(input.SkipCount).Take(input.MaxResultCount);
    }
}
