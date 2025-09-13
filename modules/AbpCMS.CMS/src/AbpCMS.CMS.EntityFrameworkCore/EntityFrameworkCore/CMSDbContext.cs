using AbpCMS.CMS.Core;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace AbpCMS.CMS.EntityFrameworkCore;

[ConnectionStringName(CMSDbProperties.ConnectionStringName)]
public class CMSDbContext : AbpDbContext<CMSDbContext>, ICMSDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */


    // CMS Entities
    public DbSet<Page> Pages { get; set; }
    public DbSet<PageCategory> PageCategories { get; set; }
    public DbSet<PageCategoryMapping> PageCategoryMappings { get; set; }



    public CMSDbContext(DbContextOptions<CMSDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureCMS();
    }
}
