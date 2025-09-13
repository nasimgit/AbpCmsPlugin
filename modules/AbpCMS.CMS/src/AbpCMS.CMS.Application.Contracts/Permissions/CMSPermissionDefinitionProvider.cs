using AbpCMS.CMS.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace AbpCMS.CMS.Permissions;

public class CMSPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var cmsGroup = context.AddGroup(CMSPermissions.GroupName, L("Permission:CMS"));

        // Pages permissions
        var pagesPermission = cmsGroup.AddPermission(CMSPermissions.Pages.Default, L("Permission:Pages"));
        pagesPermission.AddChild(CMSPermissions.Pages.Create, L("Permission:Pages.Create"));
        pagesPermission.AddChild(CMSPermissions.Pages.Edit, L("Permission:Pages.Edit"));
        pagesPermission.AddChild(CMSPermissions.Pages.Delete, L("Permission:Pages.Delete"));
        pagesPermission.AddChild(CMSPermissions.Pages.Publish, L("Permission:Pages.Publish"));

        // Categories permissions
        var categoriesPermission = cmsGroup.AddPermission(CMSPermissions.Categories.Default, L("Permission:Categories"));
        categoriesPermission.AddChild(CMSPermissions.Categories.Create, L("Permission:Categories.Create"));
        categoriesPermission.AddChild(CMSPermissions.Categories.Edit, L("Permission:Categories.Edit"));
        categoriesPermission.AddChild(CMSPermissions.Categories.Delete, L("Permission:Categories.Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<CMSResource>(name);
    }
}
