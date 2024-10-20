using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly IImageWorker _imageWorker;
        //DI - Depencecy Injection
        public ProductsController(AppBimbaDbContext context, IMapper mapper, IImageWorker imageWorker)
        {
            _dbContext = context;
            _mapper = mapper;
            _imageWorker = imageWorker;
        }
        public IActionResult Index()
        {
            List<ProductItemViewModel> model = _dbContext.Products
                .ProjectTo<ProductItemViewModel>(_mapper.ConfigurationProvider)
                .ToList();
            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var product = _dbContext.Products.Include(x => x.ProductImages).SingleOrDefault(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            if (product.ProductImages != null)
            {
                foreach (var productImage in product.ProductImages)
                {
                    _imageWorker.Delete(productImage.Image);
                    _dbContext.ProductImages.Remove(productImage);

                }
            }
            _dbContext.Products.Remove(product);
            _dbContext.SaveChanges();

            return Json(new { text = "Ми його видалили" }); // Вертаю об'єкт у відповідь
        }

    }
}
