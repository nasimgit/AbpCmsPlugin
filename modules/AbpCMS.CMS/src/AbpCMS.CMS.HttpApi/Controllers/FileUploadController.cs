using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using System.IO;
using AbpCMS.CMS.Core;
using AbpCMS.CMS.Permissions;

namespace AbpCMS.CMS.Controllers;

[ApiController]
[Route("api/upload")]
[Authorize]
public class FileUploadController : AbpControllerBase
{
    private readonly IFileAppService _fileAppService;

    public FileUploadController(IFileAppService fileAppService)
    {
        _fileAppService = fileAppService;
    }

    [HttpPost]
    [Route("image")]
    [Authorize(CMSPermissions.Pages.Edit)]
    public async Task<FileUploadResultDto> UploadImage()
    {
        var file = Request.Form.Files[0];
        
        if (file == null || file.Length == 0)
        {
            throw new UserFriendlyException("No file uploaded");
        }

        // Convert IFormFile to byte array
        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        var fileBytes = memoryStream.ToArray();

        return await _fileAppService.UploadImageAsync(fileBytes, file.FileName, file.ContentType);
    }

    [HttpDelete]
    [Route("image/{fileName}")]
    [Authorize(CMSPermissions.Pages.Edit)]
    public async Task<FileDeleteResultDto> DeleteImage(string fileName)
    {
        return await _fileAppService.DeleteImageAsync(fileName);
    }

    [HttpGet]
    [Route("images")]
    [Authorize(CMSPermissions.Pages.Default)]
    public async Task<List<FileInfoDto>> GetUploadedImages()
    {
        return await _fileAppService.GetUploadedImagesAsync();
    }
}
