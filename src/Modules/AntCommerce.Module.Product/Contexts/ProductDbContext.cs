namespace AntCommerce.Module.Product.Contexts
{
    using Microsoft.EntityFrameworkCore;

    public partial class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Product>(entity =>
            {
                entity.ToTable("Products");
                entity.HasKey(x => x.Id);
                entity.Property(x => x.SKU).IsRequired();
                entity.Property(x => x.Name).IsRequired();
                entity.Property(x => x.Price).HasColumnType("decimal(18,4)");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}