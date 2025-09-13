using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace AbpCMS.CMS;

[DependsOn(
    typeof(CMSDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class CMSApplicationContractsModule : AbpModule
{

}
