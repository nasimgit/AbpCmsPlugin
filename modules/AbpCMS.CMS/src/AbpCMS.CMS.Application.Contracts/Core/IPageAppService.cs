using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace AbpCMS.CMS.Core;

public interface IPageAppService : IApplicationService
{
    Task<PageDto> GetAsync(Guid id);
    Task<PagedResultDto<PageDto>> GetListAsync(GetPageListDto input);
    Task<PageDto> CreateAsync(CreatePageDto input);
    Task<PageDto> UpdateAsync(Guid id, UpdatePageDto input);
    Task DeleteAsync(Guid id);
    Task<PageDto> GetBySlugAsync(string slug);
    Task<PageDto> PublishAsync(Guid id);
    Task<PageDto> UnpublishAsync(Guid id);
    Task<PagedResultDto<PageDto>> GetPublishedPagesAsync(GetPageListDto input);
    Task<PageDto?> GetHomePageAsync();
    Task<PageDto> SetAsHomePageAsync(Guid id);
}
