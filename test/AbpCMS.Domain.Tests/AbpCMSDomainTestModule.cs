using Volo.Abp.Modularity;

namespace AbpCMS;

[DependsOn(
    typeof(AbpCMSDomainModule),
    typeof(AbpCMSTestBaseModule)
)]
public class AbpCMSDomainTestModule : AbpModule
{

}
