namespace PR1_ASP.Models;

public class HomeIndexViewModel
{
    public IReadOnlyList<ProductCatalogViewModel> FeaturedProducts { get; init; } = Array.Empty<ProductCatalogViewModel>();
}
