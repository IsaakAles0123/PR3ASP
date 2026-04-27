namespace PR1_ASP.Models;

public static class ProductDescriptionText
{
    /// <summary>
    /// Короткое описание для каталога/корзины (в БД отдельного поля нет — поэтому текстом).
    /// </summary>
    public static string Short(string productName, string categoryName, int stock)
    {
        var name = productName.Trim();
        var cat = string.IsNullOrWhiteSpace(categoryName) ? "-" : categoryName.Trim();

        // Небольшие “человеческие” варианты, чтобы не было одной одинаковой строки у всех.
        var tail = cat switch
        {
            "Running" => "Подходит для пробежек и активных прогулок.",
            "Basketball" => "Удобная посадка для игры и тренировок.",
            "Kids" => "Лёгкая модель для повседневной носки.",
            "Women" => "Универсальный дизайн на каждый день.",
            "Men" => "Классический вариант на каждый день.",
            _ => "Универсальная модель на каждый день."
        };

        return $"{name} — {tail} Категория: {cat}. В наличии: {stock} шт.";
    }
}
