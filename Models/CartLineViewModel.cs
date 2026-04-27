namespace PR1_ASP.Models;

public class CartLineViewModel
{
    public int CartItemId { get; set; }
    public int ProductId { get; set; }

    public string ProductName { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public int MaxStock { get; set; }

    public decimal LineTotal => Price * Quantity;
}
