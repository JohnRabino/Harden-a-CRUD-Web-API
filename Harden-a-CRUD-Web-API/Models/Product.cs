namespace Harden_a_CRUD_Web_API.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public decimal SalePrice { get; set; }
    }
}