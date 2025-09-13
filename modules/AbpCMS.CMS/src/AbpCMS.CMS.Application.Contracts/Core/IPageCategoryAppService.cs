using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace AbpCMS.CMS.Core;

public interface IPageCategoryAppService : IApplicationService
{
    Task<PageCategoryDto> GetAsync(Guid id);
    Task<PagedResultDto<PageCategoryDto>> GetListAsync(GetPageCategoryListDto input);
    Task<PageCategoryDto> CreateAsync(CreatePageCategoryDto input);
    Task<PageCategoryDto> UpdateAsync(Guid id, UpdatePageCategoryDto input);
    Task DeleteAsync(Guid id);
    Task<PageCategoryDto> GetBySlugAsync(string slug);
    Task<PagedResultDto<PageCategoryDto>> GetActiveCategoriesAsync(GetPageCategoryListDto input);
    Task<PageCategoryDto> ActivateAsync(Guid id);
    Task<PageCategoryDto> DeactivateAsync(Guid id);
}
