using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace AbpCMS.CMS.Core;

public interface IPublicPageAppService : IApplicationService
{
    Task<PageDto> GetBySlugAsync(string slug);
    Task<PagedResultDto<PageDto>> GetPublishedPagesAsync(GetPageListDto input);
    Task<PagedResultDto<PageCategoryDto>> GetActiveCategoriesAsync(GetPageCategoryListDto input);
    Task<PageCategoryDto> GetCategoryBySlugAsync(string slug);
    Task<PageDto?> GetHomePageAsync();
}
