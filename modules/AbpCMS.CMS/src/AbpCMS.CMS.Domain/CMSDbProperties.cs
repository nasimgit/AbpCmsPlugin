namespace AbpCMS.CMS;

public static class CMSDbProperties
{
    public static string DbTablePrefix { get; set; } = "CMS";

    public static string? DbSchema { get; set; } = null;

    public const string ConnectionStringName = "CMS";
}
