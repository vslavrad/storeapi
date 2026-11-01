using System.ComponentModel.DataAnnotations;

namespace StoreApi.Contracts.Customers;

public sealed class CreateCustomerRequest
{
    [Required]
    [MinLength(2)]
    public required string Name { get; init; }

    [EmailAddress]
    public string? Email { get; init; }
}