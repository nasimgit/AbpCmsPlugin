using Localization.Resources.AbpUi;
using AbpCMS.CMS.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;
using AbpCMS.CMS.EntityFrameworkCore;

namespace AbpCMS.CMS;

[DependsOn(
    typeof(CMSEntityFrameworkCoreModule),
    typeof(CMSApplicationModule),
    typeof(AbpAspNetCoreMvcModule))]
public class CMSHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(CMSHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<CMSResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });

        // Conventional controllers are now configured in the main host module
        // when using PlugInSources.AddFolder instead of project references
    }
}
