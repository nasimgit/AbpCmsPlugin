using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace AbpCMS.CMS;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(CMSDomainSharedModule)
)]
public class CMSDomainModule : AbpModule
{

}
