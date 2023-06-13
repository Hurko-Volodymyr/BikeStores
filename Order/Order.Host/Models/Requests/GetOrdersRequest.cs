namespace Order.Host.Models.Requests
{
    public class GetOrdersRequest
    {
        [Required]
        public string UserId { get; set; } = null!;
    }
}
