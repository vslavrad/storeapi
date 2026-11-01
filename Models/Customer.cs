namespace StoreApi.Models;

public sealed class Customer
{
    public long Id { get; set; }

    public required string Name { get; set; }

    public string? Email { get; set; }
}