using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using WebBimba.Data;
using WebBimba.Interfaces;
using WebBimba.Models.Category;
using WebBimba.Models.Product;

namespace WebBimba.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AppBimbaDbContext _dbContext;
        private readonly IMapper _mapper;
        //DI - Depencecy Injection
        public ProductsController(AppBimbaDbContext context, IMapper mapper)
        {
            _dbContext = context;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            List<ProductItemViewModel> model = _dbContext.Products
                .ProjectTo<ProductItemViewModel>(_mapper.ConfigurationProvider)
                .ToList();
            return View(model);
        }
    }
}
