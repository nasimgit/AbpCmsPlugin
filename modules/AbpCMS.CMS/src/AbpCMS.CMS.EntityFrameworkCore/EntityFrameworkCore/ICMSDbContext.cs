using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace AbpCMS.CMS.EntityFrameworkCore;

[ConnectionStringName(CMSDbProperties.ConnectionStringName)]
public interface ICMSDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
}
