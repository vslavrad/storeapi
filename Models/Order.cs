namespace StoreApi.Models;

public sealed class Order
{
    public long Id { get; set; }

    public long CustomerId { get; set; }

    public string Status { get; set; } = "new";

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}