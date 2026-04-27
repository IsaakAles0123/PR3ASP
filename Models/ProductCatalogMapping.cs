namespace PR1_ASP.Models;

public static class ProductCatalogMapping
{
    public static ProductCatalogViewModel ToCatalogViewModel(this Product p)
    {
        var categoryName = p.Category != null ? p.Category.CategoryName : "-";
        return new ProductCatalogViewModel
        {
            ProductId = p.ProductId,
            ProductName = p.ProductName,
            Price = p.Price,
            Stock = p.Stock,
            CategoryName = categoryName,
            Description = ProductDescriptionText.Short(p.ProductName, categoryName, p.Stock)
        };
    }
}
