using AbpCMS.Samples;
using Xunit;

namespace AbpCMS.EntityFrameworkCore.Applications;

[Collection(AbpCMSTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<AbpCMSEntityFrameworkCoreTestModule>
{

}
