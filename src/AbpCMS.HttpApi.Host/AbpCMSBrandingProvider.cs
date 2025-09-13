using Microsoft.Extensions.Localization;
using AbpCMS.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace AbpCMS;

[Dependency(ReplaceServices = true)]
public class AbpCMSBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<AbpCMSResource> _localizer;

    public AbpCMSBrandingProvider(IStringLocalizer<AbpCMSResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
