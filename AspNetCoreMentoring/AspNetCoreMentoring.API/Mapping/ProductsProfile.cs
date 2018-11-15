using AspNetCoreMentoring.API.Contracts.Dto.Product;
using AspNetCoreMentoring.Infrastructure;
using AspNetCoreMentoring.Infrastructure.EfEntities;
using AutoMapper;

namespace AspNetCoreMentoring.API.Mapping
{
    public class ProductsProfile : Profile
    {
        public ProductsProfile()
        {
            this.CreateMap<ProductQueryResult, ProductReadListDto>();

            this.CreateMap<Products, ProductReadListDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
                .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.Supplier.CompanyName));

            this.CreateMap<Products, ProductWriteItemDto>()
                .ForMember(dest => dest.SelectedCategoryId, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.SelectedSupplierId, opt => opt.MapFrom(src => src.SupplierId));

            this.CreateMap<Products, ProductReadItemDto>()
                   .ForMember(dest => dest.SelectedCategoryId, opt => opt.MapFrom(src => src.CategoryId))
                   .ForMember(dest => dest.SelectedSupplierId, opt => opt.MapFrom(src => src.SupplierId));

            this.CreateMap<ProductWriteItemDto, Products>()
              .ForMember(dest => dest.SupplierId, opt => opt.MapFrom(src => src.SelectedSupplierId))
              .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.SelectedCategoryId));
        }

    }
}
