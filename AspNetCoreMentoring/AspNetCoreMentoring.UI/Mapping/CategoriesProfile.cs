using AspNetCoreMentoring.Infrastructure.EfEntities;
using AspNetCoreMentoring.UI.ViewModels.Category;
using AutoMapper;

namespace AspNetCoreMentoring.UI.Mapping
{
    public class CategoriesProfile : Profile
    {
        public CategoriesProfile()
        {
            this.CreateMap<Categories, CategoryReadListViewModel>();
            this.CreateMap<Categories, CategoryWriteItemViewModel>();


            this.CreateMap<CategoryWriteItemViewModel, Categories>();




        }
    }
}
