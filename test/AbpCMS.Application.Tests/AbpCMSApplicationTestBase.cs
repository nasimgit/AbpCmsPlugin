using Volo.Abp.Modularity;

namespace AbpCMS;

public abstract class AbpCMSApplicationTestBase<TStartupModule> : AbpCMSTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
