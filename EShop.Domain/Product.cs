using System;

namespace EShop.Domain;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Ean { get; set; } = default!;
    public decimal Price { get; set; }
    public int Stock { get; set; } = 0;
    public string Sku { get; set; } = default!;
    public bool Deleted { get; set; } = false;
    public DateTime Created_at { get; set; } = DateTime.UtcNow;
    public Guid Created_by { get; set; }
    public DateTime Uploaded_at { get; set; } = DateTime.UtcNow;
    public Guid Updated_by { get; set; }
}
