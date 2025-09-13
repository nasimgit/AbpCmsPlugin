using AbpCMS.CMS.Localization;
using Volo.Abp.Application.Services;

namespace AbpCMS.CMS;

public abstract class CMSAppService : ApplicationService
{
    protected CMSAppService()
    {
        LocalizationResource = typeof(CMSResource);
        ObjectMapperContext = typeof(CMSApplicationModule);
    }
}
