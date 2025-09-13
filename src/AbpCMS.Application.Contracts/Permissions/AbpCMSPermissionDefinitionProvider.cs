using AbpCMS.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace AbpCMS.Permissions;

public class AbpCMSPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var cmsGroup = context.AddGroup(AbpCMSPermissions.GroupName, L("Permission:AbpCMS"));

    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AbpCMSResource>(name);
    }
}
