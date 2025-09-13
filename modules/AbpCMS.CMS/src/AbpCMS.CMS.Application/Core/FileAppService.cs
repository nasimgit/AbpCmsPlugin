using AbpCMS.CMS.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace AbpCMS.CMS.Core;

[Authorize(CMSPermissions.Pages.Edit)]
public class FileAppService : ApplicationService, IFileAppService
{
    private readonly string _uploadPath;

    public FileAppService()
    {
        // Create uploads directory if it doesn't exist
        _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "images");
        if (!Directory.Exists(_uploadPath))
        {
            Directory.CreateDirectory(_uploadPath);
        }
    }

    public async Task<FileUploadResultDto> UploadImageAsync(byte[] fileBytes, string fileName, string contentType)
    {
        if (fileBytes == null || fileBytes.Length == 0)
        {
            throw new UserFriendlyException("No file uploaded");
        }

        // Validate file type
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        var fileExtension = Path.GetExtension(fileName).ToLowerInvariant();
        
        if (!allowedExtensions.Contains(fileExtension))
        {
            throw new UserFriendlyException("Invalid file type. Only JPG, JPEG, PNG, GIF, and WebP files are allowed.");
        }

        // Validate file size (5MB limit)
        if (fileBytes.Length > 5 * 1024 * 1024)
        {
            throw new UserFriendlyException("File size exceeds 5MB limit");
        }

        // Generate unique filename
        var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
        var filePath = Path.Combine(_uploadPath, uniqueFileName);

        // Save file
        await File.WriteAllBytesAsync(filePath, fileBytes);

        return new FileUploadResultDto
        {
            Success = true,
            FileName = uniqueFileName,
            OriginalName = fileName,
            Url = $"/uploads/images/{uniqueFileName}",
            Size = fileBytes.Length
        };
    }

    public async Task<FileDeleteResultDto> DeleteImageAsync(string fileName)
    {
        var filePath = Path.Combine(_uploadPath, fileName);
        
        if (!File.Exists(filePath))
        {
            throw new UserFriendlyException("File not found");
        }

        try
        {
            File.Delete(filePath);
            return new FileDeleteResultDto
            {
                Success = true,
                Message = "File deleted successfully"
            };
        }
        catch (Exception ex)
        {
            return new FileDeleteResultDto
            {
                Success = false,
                Message = $"Error deleting file: {ex.Message}"
            };
        }
    }

    public async Task<List<FileInfoDto>> GetUploadedImagesAsync()
    {
        var files = new List<FileInfoDto>();
        
        if (!Directory.Exists(_uploadPath))
        {
            return files;
        }

        var fileInfos = Directory.GetFiles(_uploadPath)
            .Select(filePath => new FileInfo(filePath))
            .OrderByDescending(f => f.CreationTime)
            .ToList();

        foreach (var fileInfo in fileInfos)
        {
            files.Add(new FileInfoDto
            {
                FileName = fileInfo.Name,
                Url = $"/uploads/images/{fileInfo.Name}",
                Size = fileInfo.Length,
                Created = DateTime.Now, 
            });
        }

        return files;
    }
}
