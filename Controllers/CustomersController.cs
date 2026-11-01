using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreApi.Contracts.Customers;
using StoreApi.Data;
using StoreApi.Models;


namespace StoreApi.Controllers;

[ApiController]
[Route("api/customers")]
public sealed class CustomersController : ControllerBase
{
    private readonly AppDbContext _db;

    public CustomersController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Customer>>> GetAll()
    {
        List<Customer> customers = await _db.Customers.AsNoTracking().ToListAsync();
        return Ok(customers);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<Customer>> GetById(long id)
    {
        Customer? customer = await _db.Customers.AsNoTracking().SingleOrDefaultAsync(customer => customer.Id == id);

        if (customer is null) return NotFound();

        return Ok(customer);
    }

    [HttpPost]
    public async Task<ActionResult<Customer>> Create([FromBody] CreateCustomerRequest request)
    {
        var customer = new Customer
        {
            Name = request.Name,
            Email = request.Email
        };

        _db.Customers.Add(customer);

        await _db.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetById),
            new { id = customer.Id },
            customer);
    }
}