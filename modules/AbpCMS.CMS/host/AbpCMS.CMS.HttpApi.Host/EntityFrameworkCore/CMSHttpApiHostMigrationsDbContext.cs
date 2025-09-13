using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace AbpCMS.CMS.EntityFrameworkCore;

public class CMSHttpApiHostMigrationsDbContext : AbpDbContext<CMSHttpApiHostMigrationsDbContext>
{
    public CMSHttpApiHostMigrationsDbContext(DbContextOptions<CMSHttpApiHostMigrationsDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureCMS();
    }
}
