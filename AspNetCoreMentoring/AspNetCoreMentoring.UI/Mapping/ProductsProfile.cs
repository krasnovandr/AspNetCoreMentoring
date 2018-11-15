using AspNetCoreMentoring.Infrastructure;
using AspNetCoreMentoring.Infrastructure.EfEntities;
using AspNetCoreMentoring.UI.ViewModels.Product;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreMentoring.UI.Mapping
{
    public class ProductsProfile : Profile
    {
        public ProductsProfile()
        {
            this.CreateMap<ProductQueryResult, ProductReadListViewModel>();

            this.CreateMap<Products, ProductReadListViewModel>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
                .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.Supplier.CompanyName));

            this.CreateMap<Products, ProductWriteItemViewModel>()
                .ForMember(dest => dest.SelectedCategoryId, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.SelectedSupplierId, opt => opt.MapFrom(src => src.SupplierId));

            this.CreateMap<ProductWriteItemViewModel, Products>()
              .ForMember(dest => dest.SupplierId, opt => opt.MapFrom(src => src.SelectedSupplierId))
              .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.SelectedCategoryId));
        }

    }
}
