using AbpCMS.CMS.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace AbpCMS.CMS;

public abstract class CMSController : AbpControllerBase
{
    protected CMSController()
    {
        LocalizationResource = typeof(CMSResource);
    }
}
