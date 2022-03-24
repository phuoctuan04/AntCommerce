namespace AntCommerce.Module.Product.Contexts.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string SKU { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public decimal? Price { get; set; }

        public DateTime CreateAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdateAt { get; set;}

        public string? CreatedBy { get; set; }

        public string? UpdatedBy { get; set; }
    }
}
