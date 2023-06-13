namespace Catalog.Host.Models.Requests.Items;

public class PaginatedItemsRequest
{
    public int PageIndex { get; set; }

    public int PageSize { get; set; }
}