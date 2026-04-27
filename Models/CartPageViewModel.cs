namespace PR1_ASP.Models;

public class CartPageViewModel
{
    public List<CartLineViewModel> Lines { get; set; } = new();

    public decimal GrandTotal => Lines.Sum(l => l.LineTotal);
}
