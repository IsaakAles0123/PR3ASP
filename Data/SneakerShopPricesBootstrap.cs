using Microsoft.EntityFrameworkCore;
using PR1_ASP.Models;

namespace PR1_ASP.Data;

/// <summary>
/// Поднимает заниженные «учебные» цены до витринных (по имени товара). Идемпотентно: если цены уже нормальные, не трогает.
/// Дублирует логику скрипта Scripts/UpdateProductPrices.sql для удобства при локальном запуске.
/// </summary>
public static class SneakerShopPricesBootstrap
{
    private static readonly (string Name, decimal Price)[] Canonical = new[]
    {
        ("Air Jordan 1", 12_990m),
        ("Adidas Ultraboost", 11_990m),
        ("New Balance 574", 9_990m),
        ("Reebok Classic", 7_490m),
    };

    /// <summary>Порог «явно не витринная» цена — только такие строки обновляем автоматически.</summary>
    private const decimal DemoPriceCeiling = 500m;

    public static async Task ApplyDemoPriceFixesAsync(SneakerShopContext db, CancellationToken cancellationToken = default)
    {
        var changed = false;
        foreach (var (name, price) in Canonical)
        {
            var product = await db.Products
                .FirstOrDefaultAsync(p => p.ProductName == name, cancellationToken)
                .ConfigureAwait(false);

            if (product is null || product.Price > DemoPriceCeiling)
                continue;

            product.Price = price;
            changed = true;
        }

        if (changed)
            await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
}
