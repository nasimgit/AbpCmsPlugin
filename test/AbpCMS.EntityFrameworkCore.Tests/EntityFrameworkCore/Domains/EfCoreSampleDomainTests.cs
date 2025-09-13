using AbpCMS.Samples;
using Xunit;

namespace AbpCMS.EntityFrameworkCore.Domains;

[Collection(AbpCMSTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<AbpCMSEntityFrameworkCoreTestModule>
{

}
