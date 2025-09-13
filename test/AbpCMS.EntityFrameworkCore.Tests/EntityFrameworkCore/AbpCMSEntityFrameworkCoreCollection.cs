using Xunit;

namespace AbpCMS.EntityFrameworkCore;

[CollectionDefinition(AbpCMSTestConsts.CollectionDefinitionName)]
public class AbpCMSEntityFrameworkCoreCollection : ICollectionFixture<AbpCMSEntityFrameworkCoreFixture>
{

}
