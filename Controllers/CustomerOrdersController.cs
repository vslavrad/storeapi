using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreApi.Contracts.Orders;
using StoreApi.Data;
using StoreApi.Models;

namespace StoreApi.Controllers;

[ApiController]
[Route("api/customers/{customerId:long}/orders")]
public sealed class CustomerOrdersController : ControllerBase
{
    private readonly AppDbContext _db;

    public CustomerOrdersController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderResponse>>> GetAll(
        long customerId
    )
    {
        bool customerExists = await _db.Customers.AnyAsync(customer => customer.Id == customerId);

        if (!customerExists) return NotFound();

        List<OrderResponse> orders = await _db.Orders
            .AsNoTracking()
            .Where(order => order.CustomerId == customerId)
            .OrderByDescending(order => order.CreatedAt)
            .Select(order => new OrderResponse(
                order.Id,
                order.CustomerId,
                order.Status,
                order.CreatedAt))
            .ToListAsync();

        return Ok(orders);
    }

    [HttpPost]
    public async Task<ActionResult<OrderResponse>> Create(
        long customerId
    )
    {
        bool customerExists = await _db.Customers.AnyAsync(customer => customer.Id == customerId);

        if (!customerExists) return NotFound();

        var order = new Order
        {
            CustomerId = customerId
        };

        _db.Orders.Add(order);

        await _db.SaveChangesAsync();

        var response = new OrderResponse(
            order.Id,
            order.CustomerId,
            order.Status,
            order.CreatedAt
        );

        return CreatedAtAction(
            nameof(OrdersController.GetById),
            "Orders",
            new { id = order.Id },
            response
        );
    }
}