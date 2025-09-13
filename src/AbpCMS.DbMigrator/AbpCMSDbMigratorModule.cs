using AbpCMS.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace AbpCMS.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpCMSEntityFrameworkCoreModule),
    typeof(AbpCMSApplicationContractsModule)
)]
public class AbpCMSDbMigratorModule : AbpModule
{
}
