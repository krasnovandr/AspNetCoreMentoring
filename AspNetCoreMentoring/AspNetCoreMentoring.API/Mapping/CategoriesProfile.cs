using AspNetCoreMentoring.API.Contracts.Dto.Category;
using AspNetCoreMentoring.Infrastructure.EfEntities;
using AutoMapper;

namespace AspNetCoreMentoring.API.Mapping
{
    public class CategoriesProfile : Profile
    {
        public CategoriesProfile()
        {
            this.CreateMap<Categories, CategoryReadListDto>();
            this.CreateMap<Categories, CategoryWriteItemDto>();

            this.CreateMap<CategoryWriteItemDto, Categories>();
        }
    }
}
