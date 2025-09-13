using Volo.Abp.Modularity;

namespace AbpCMS;

/* Inherit from this class for your domain layer tests. */
public abstract class AbpCMSDomainTestBase<TStartupModule> : AbpCMSTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
