using AutoMapper;
using WebBimba.Data.Entities;
using WebBimba.Models.Category;

namespace WebBimba.Mapper
{
    public class AppMapperProfile : Profile
    {
        public AppMapperProfile()
        {
            CreateMap<CategoryEntity, CategoryItemViewModel>();
            CreateMap<CategoryCreateViewModel, CategoryEntity>();
        }
    }
}
