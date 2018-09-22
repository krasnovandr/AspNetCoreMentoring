using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreMentoring.Infrastructure.EfEntities;
using AspNetCoreMentoring.UI.ViewModels;
using AspNetCoreMentoring.UI.ViewModels.Category;
using AspNetCoreMentoring.UI.ViewModels.Product;
using AutoMapper;

namespace AspNetCoreMentoring.UI.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<Categories, CategoryReadListViewModel>();
            this.CreateMap<Categories, CategoryItemViewModel>();

            this.CreateMap<Products, ProductReadListViewModel>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
                .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.Supplier.CompanyName));

            this.CreateMap<Products, ProductWriteItemViewModel>()
                .ForMember(dest => dest.SelectedCategoryId, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.SelectedSupplierId, opt => opt.MapFrom(src => src.SupplierId));

        }
    }
}
