using System.ComponentModel.DataAnnotations;

namespace StoreApi.Contracts.Orders;

public sealed class UpdateOrderStatusRequest
{
    [Required]
    [MaxLength(30)]
    public required string Status { get; init; }
}