using AbpCMS.CMS.Core;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace AbpCMS.CMS.EntityFrameworkCore;

public static class CMSDbContextModelCreatingExtensions
{
    public static void ConfigureCMS(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.Entity<Page>(b =>
        {
            b.ToTable(CMSDbProperties.DbTablePrefix + "Pages", CMSDbProperties.DbSchema);
            b.ConfigureByConvention();

            b.Property(x => x.Title).IsRequired().HasMaxLength(200);
            b.Property(x => x.Slug).IsRequired().HasMaxLength(200);
            b.Property(x => x.Content).IsRequired();
            b.Property(x => x.MetaTitle).HasMaxLength(200);
            b.Property(x => x.MetaDescription).HasMaxLength(500);
            b.Property(x => x.MetaKeywords).HasMaxLength(200);
            b.Property(x => x.Author).HasMaxLength(100);
            b.Property(x => x.Tags).HasMaxLength(500);
            b.Property(x => x.IsHomePage).HasDefaultValue(false);

            b.HasIndex(x => x.Slug).IsUnique();
            b.HasIndex(x => new { x.Slug, x.TenantId }).IsUnique();
            b.HasIndex(x => x.IsHomePage);
        });

        builder.Entity<PageCategory>(b =>
        {
            b.ToTable(CMSDbProperties.DbTablePrefix + "PageCategories", CMSDbProperties.DbSchema);
            b.ConfigureByConvention();

            b.Property(x => x.Name).IsRequired().HasMaxLength(100);
            b.Property(x => x.Slug).IsRequired().HasMaxLength(100);
            b.Property(x => x.Description).HasMaxLength(500);
            b.Property(x => x.Color).HasMaxLength(20);
            b.Property(x => x.Icon).HasMaxLength(50);

            b.HasIndex(x => x.Slug).IsUnique();
            b.HasIndex(x => new { x.Slug, x.TenantId }).IsUnique();
        });

        builder.Entity<PageCategoryMapping>(b =>
        {
            b.ToTable(CMSDbProperties.DbTablePrefix + "PageCategoryMappings", CMSDbProperties.DbSchema);
            b.ConfigureByConvention();

            b.HasIndex(x => new { x.PageId, x.CategoryId }).IsUnique();
        });
    }
}
