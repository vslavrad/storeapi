using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreApi.Contracts.Orders;
using StoreApi.Data;
using StoreApi.Models;

namespace StoreApi.Controllers;

[ApiController]
[Route("api/orders")]
public sealed class OrdersController : ControllerBase
{
    private readonly AppDbContext _db;

    public OrdersController(AppDbContext db)
    {
        _db = db;
    }

    private static readonly HashSet<string> AllowedStatuses = new(StringComparer.OrdinalIgnoreCase)
    {
        "new",
        "paid",
        "completed",
        "cancelled"
    };

    [HttpGet("{id:long}")]
    public async Task<ActionResult<OrderResponse>> GetById(long id)
    {
        OrderResponse? order = await _db.Orders
            .AsNoTracking()
            .Where(order => order.Id == id)
            .Select(order => new OrderResponse(
                order.Id,
                order.CustomerId,
                order.Status,
                order.CreatedAt))
            .SingleOrDefaultAsync();
            
        if (order is null) return NotFound();
        
        return Ok(order);
    }

    [HttpPatch("{id:long}/status")]
    public async Task<ActionResult<OrderResponse>> UpdateStatus(
        long id,
        [FromBody] UpdateOrderStatusRequest request
    )
    {
        string normalizedStatus = request.Status.Trim().ToLowerInvariant();

        if (!AllowedStatuses.Contains(normalizedStatus))
        {
            return BadRequest(new
            {
                error = "Unknown order status.",
                allowedStatuses = AllowedStatuses
            });
        }

        Order? order = await _db.Orders.SingleOrDefaultAsync(order => order.Id == id);

        if (order is null) return NotFound();

        order.Status = normalizedStatus;

        await _db.SaveChangesAsync();

        var response = new OrderResponse(
            order.Id,
            order.CustomerId,
            order.Status,
            order.CreatedAt
        );

        return Ok(response);
    }
}