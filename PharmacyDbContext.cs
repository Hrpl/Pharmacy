using Microsoft.EntityFrameworkCore;
using Pharmacy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy;

public class PharmacyDbContext : DbContext
{
    public DbSet<Role> Roles { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Medicine> Medicines { get; set; }
    public DbSet<MedicInOrder> MedicInOrders { get; set; }
    public DbSet<MedicInRequest> MedicInRequest { get; set; }
    public DbSet<Order> Order { get; set; }
    public DbSet<Request> Request { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb; Database=PharmApp; Trusted_Connection=True;");
    }

    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasIndex(e => e.RoleId, "IX_Employees_RoleId");

            entity.HasOne(d => d.Role).WithMany(p => p.Employees).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<MedicInOrder>(entity =>
        {
            entity.HasIndex(e => e.MedicineId, "IX_MedicInOrders_MedicineId");

            entity.HasIndex(e => e.OrderId, "IX_MedicInOrders_OrderId");

            entity.HasOne(d => d.Medicine).WithMany(p => p.MedicInOrders).HasForeignKey(d => d.MedicineId);

            entity.HasOne(d => d.Order).WithMany(p => p.MedicInOrders).HasForeignKey(d => d.OrderId);
        });

        modelBuilder.Entity<MedicInRequest>(entity =>
        {
            entity.ToTable("MedicInRequest");

            entity.HasIndex(e => e.MedicineId, "IX_MedicInRequest_MedicineId");

            entity.HasIndex(e => e.RequestNumber, "IX_MedicInRequest_RequestNumber");

            entity.HasOne(d => d.Medicine).WithMany(p => p.MedicInRequests).HasForeignKey(d => d.MedicineId);

        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Order");
        });

        modelBuilder.Entity<Request>(entity =>
        {
            entity.ToTable("Request");

            entity.Property(e => e.Status).HasDefaultValue("");
        });

    }

}
