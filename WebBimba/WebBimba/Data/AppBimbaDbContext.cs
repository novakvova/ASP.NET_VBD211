using Microsoft.EntityFrameworkCore;
using WebBimba.Data.Entities;

namespace WebBimba.Data
{
    public class AppBimbaDbContext : DbContext
    {
        public AppBimbaDbContext(DbContextOptions<AppBimbaDbContext> options)
            : base(options) { }

        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<ProductImageEntity> ProductImages { get; set; }
    }
}
