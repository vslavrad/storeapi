namespace StoreApi.Contracts.Orders;

public sealed record OrderResponse(
    long Id,
    long CustomerId,
    string Status,
    DateTimeOffset CreatedAt
);