using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace AbpCMS.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class AbpCMSDbContextFactory : IDesignTimeDbContextFactory<AbpCMSDbContext>
{
    public AbpCMSDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();
        
        AbpCMSEfCoreEntityExtensionMappings.Configure();

        var builder = new DbContextOptionsBuilder<AbpCMSDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Default"));
        
        return new AbpCMSDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../AbpCMS.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
