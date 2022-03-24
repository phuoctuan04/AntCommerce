namespace AntCommerce.Module.Product.DTOs
{
    public class ProductQueryModel
    {
        public string? Name { get; set; }

        public string? Sku { get; set; }

        public decimal? FromPrice { get; set; }

        public decimal? ToPrice { get; set; }

        public int Page { get; set; } = 1;

        public int Limit { get; set; } = 25;
    }
}
