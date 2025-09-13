using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace AbpCMS.CMS.EntityFrameworkCore;

public class CMSHttpApiHostMigrationsDbContextFactory : IDesignTimeDbContextFactory<CMSHttpApiHostMigrationsDbContext>
{
    public CMSHttpApiHostMigrationsDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<CMSHttpApiHostMigrationsDbContext>()
            .UseSqlServer(configuration.GetConnectionString("CMS"));

        return new CMSHttpApiHostMigrationsDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
