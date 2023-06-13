namespace Order.Host.Models
{
    public class BasketItemModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Region { get; set; } = null!;

        public string PictureUrl { get; set; } = null!;

        public decimal Price { get; set; }

        public int Count { get; set; }
    }
}