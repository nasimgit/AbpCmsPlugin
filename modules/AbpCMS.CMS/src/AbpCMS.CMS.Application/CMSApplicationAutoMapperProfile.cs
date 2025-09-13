using AbpCMS.CMS.Core;
using AutoMapper;

namespace AbpCMS.CMS;

public class CMSApplicationAutoMapperProfile : Profile
{
    public CMSApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        // CMS Mappings
        CreateMap<Page, PageDto>();
        CreateMap<PageCategory, PageCategoryDto>();
    }
}
