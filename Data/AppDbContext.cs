using Microsoft.EntityFrameworkCore;
using StoreApi.Models;

namespace StoreApi.Data;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }


    public DbSet<Customer> Customers 
    {
        get
        { 
            return Set<Customer>();
        }
    }

    public DbSet<Order> Orders => Set<Order>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("customers");

            entity.HasKey(customer => customer.Id);

            entity.Property(customer => customer.Id)
                .HasColumnName("id");

            entity.Property(customer => customer.Name)
                .HasColumnName("name")
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(customer => customer.Email)
                .HasColumnName("email")
                .HasMaxLength(320);

            entity.HasIndex(customer => customer.Email)
                .IsUnique();
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("orders");

            entity.HasKey(order => order.Id);

            entity.Property(order => order.Id).HasColumnName("id");

            entity.Property(order => order.CustomerId).HasColumnName("customer_id");

            entity.Property(order => order.Status).HasColumnName("status")
            .HasMaxLength(30)
            .IsRequired();

            entity.Property(order => order.CreatedAt).HasColumnName("created_at")
            .IsRequired();

            entity.HasOne<Customer>().WithMany().HasForeignKey(order => order.CustomerId).OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(order => order.CustomerId);
        });
    }
}