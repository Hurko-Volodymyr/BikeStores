namespace Order.Host.Models.Requests
{
    public class AddOrderRequest
    {
        [Required]
        public string UserId { get; set; } = null!;

        [Required]
        public string GameAccountId { get; set; } = null!;

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string LastName { get; set; } = null!;

        public BasketItemModel[] BasketItems { get; set; } = null!;

        [Required]
        [EmailAddress(ErrorMessage = "Incorrent email format")]
        public string Email { get; set; } = null!;

        public decimal TotalSum { get; set; }
    }
}
