using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using AbpCMS.CMS.Permissions;

namespace AbpCMS.CMS.Core;

[Authorize(CMSPermissions.Categories.Default)]
public class PageCategoryAppService : ApplicationService, IPageCategoryAppService
{
    private readonly PageManager _pageManager;
    private readonly IRepository<PageCategory, Guid> _repository;

    public PageCategoryAppService(
        IRepository<PageCategory, Guid> repository,
        PageManager pageManager)
    {
        _pageManager = pageManager;
        _repository = repository;
    }

    public async Task<PageCategoryDto> GetAsync(Guid id)
    {
        var category = await _repository.GetAsync(id);
        return ObjectMapper.Map<PageCategory, PageCategoryDto>(category);
    }

    public async Task<PagedResultDto<PageCategoryDto>> GetListAsync(GetPageCategoryListDto input)
    {
        var query = await _repository.GetQueryableAsync();

        if (!string.IsNullOrWhiteSpace(input.Filter))
        {
            query = query.Where(c => c.Name.Contains(input.Filter) || c.Description.Contains(input.Filter));
        }

        if (input.ParentId.HasValue)
        {
            query = query.Where(c => c.ParentId == input.ParentId.Value);
        }

        var totalCount = await AsyncExecuter.CountAsync(query);

        query = query.OrderBy(c => c.CreationTime);
        query = query.Skip(input.SkipCount).Take(input.MaxResultCount);

        var categories = await AsyncExecuter.ToListAsync(query);

        return new PagedResultDto<PageCategoryDto>(
            totalCount,
            ObjectMapper.Map<List<PageCategory>, List<PageCategoryDto>>(categories)
        );
    }

    public async Task<PageCategoryDto> GetBySlugAsync(string slug)
    {
        var category = await _repository.FirstOrDefaultAsync(c => c.Slug == slug);
        return ObjectMapper.Map<PageCategory, PageCategoryDto>(category);
    }

    public async Task<PagedResultDto<PageCategoryDto>> GetActiveCategoriesAsync(GetPageCategoryListDto input)
    {
        var query = await _repository.GetQueryableAsync();
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

        query = query.OrderBy(c => c.CreationTime);
        query = query.Skip(input.SkipCount).Take(input.MaxResultCount);

        var categories = await AsyncExecuter.ToListAsync(query);

        return new PagedResultDto<PageCategoryDto>(
            totalCount,
            ObjectMapper.Map<List<PageCategory>, List<PageCategoryDto>>(categories)
        );
    }

    [Authorize(CMSPermissions.Categories.Edit)]
    public async Task<PageCategoryDto> ActivateAsync(Guid id)
    {
        var category = await _repository.GetAsync(id);
        category.Activate();
        var updatedCategory = await _repository.UpdateAsync(category);
        return ObjectMapper.Map<PageCategory, PageCategoryDto>(updatedCategory);
    }

    [Authorize(CMSPermissions.Categories.Edit)]
    public async Task<PageCategoryDto> DeactivateAsync(Guid id)
    {
        var category = await _repository.GetAsync(id);
        category.Deactivate();
        var updatedCategory = await _repository.UpdateAsync(category);
        return ObjectMapper.Map<PageCategory, PageCategoryDto>(updatedCategory);
    }

    [Authorize(CMSPermissions.Categories.Create)]
    public async Task<PageCategoryDto> CreateAsync(CreatePageCategoryDto input)
    {
        var category = await _pageManager.CreateCategoryAsync(
            input.Name,
            input.Slug,
            input.Description,
            input.Color,
            input.Icon,
            input.SortOrder,
            input.ParentId
        );
        return ObjectMapper.Map<PageCategory, PageCategoryDto>(category);
    }

    [Authorize(CMSPermissions.Categories.Edit)]
    public async Task<PageCategoryDto> UpdateAsync(Guid id, UpdatePageCategoryDto input)
    {
        var category = await _pageManager.UpdateCategoryAsync(
            id,
            input.Name,
            input.Slug,
            input.Description,
            input.Color,
            input.Icon,
            input.SortOrder,
            input.ParentId
        );
        return ObjectMapper.Map<PageCategory, PageCategoryDto>(category);
    }

    [Authorize(CMSPermissions.Categories.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

}
