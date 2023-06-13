namespace Order.Host.Models.Requests
{
    public class CreateOrderRequest
    {
        [Required]
        public string UserId { get; set; } = null!;

        [Required]
        public string GameAccountId { get; set; } = null!;

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string LastName { get; set; } = null!;

        [Required]
        [EmailAddress(ErrorMessage = "Incorrent email format")]
        public string Email { get; set; } = null!;
    }
}
