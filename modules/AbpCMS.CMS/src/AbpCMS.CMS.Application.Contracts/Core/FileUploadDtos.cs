using System;
using System.Collections.Generic;

namespace AbpCMS.CMS.Core;

public class FileUploadResultDto
{
    public bool Success { get; set; }
    public string Url { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public string OriginalName { get; set; } = string.Empty;
    public long Size { get; set; }
}

public class FileDeleteResultDto
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}

public class FileInfoDto
{
    public string FileName { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public long Size { get; set; }
    public DateTime Created { get; set; }
}
