using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace AbpCMS.CMS.Core;

public interface IFileAppService : IApplicationService
{
    Task<FileUploadResultDto> UploadImageAsync(byte[] fileBytes, string fileName, string contentType);
    Task<FileDeleteResultDto> DeleteImageAsync(string fileName);
    Task<List<FileInfoDto>> GetUploadedImagesAsync();
}
