using Volo.Abp.Reflection;

namespace AbpCMS.CMS.Permissions;

public class CMSPermissions
{
    public const string GroupName = "CMS";

    public static class Pages
    {
        public const string Default = GroupName + ".Pages";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
        public const string Publish = Default + ".Publish";
    }

    public static class Categories
    {
        public const string Default = GroupName + ".Categories";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(CMSPermissions));
    }
}
