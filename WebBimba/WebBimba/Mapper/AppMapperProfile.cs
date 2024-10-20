using AutoMapper;
using System.Globalization;
using WebBimba.Data.Entities;
using WebBimba.Models.Category;
using WebBimba.Models.Product;

namespace WebBimba.Mapper
{
    public class AppMapperProfile : Profile
    {
        public AppMapperProfile()
        {
            CreateMap<CategoryEntity, CategoryItemViewModel>();
            CreateMap<CategoryCreateViewModel, CategoryEntity>();

            CreateMap<ProductEntity, ProductItemViewModel>()
                .ForMember(x => x.Images, opt => opt.MapFrom(p => p.ProductImages.Select(x => x.Image).ToList()))
                .ForMember(x => x.CategoryName, opt => opt.MapFrom(c => c.Category.Name));

            //CreateMap<ProductCreateViewModel, ProductEntity>()
            //    .ForMember(x => x.Price, opt => opt.MapFrom(p => 
            //    {
            //        return Decimal.Parse(p.Price, CultureInfo.InvariantCulture); 
            //    }));
        }
    }
}
