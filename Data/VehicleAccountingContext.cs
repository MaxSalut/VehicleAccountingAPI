using Microsoft.EntityFrameworkCore;
using VehicleAccountingAPI.Models;

namespace VehicleAccountingAPI.Data
{
    public class VehicleAccountingContext : DbContext
    {
        public VehicleAccountingContext(DbContextOptions<VehicleAccountingContext> options)
            : base(options)
        {
        }

        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<VehicleType> VehicleTypes { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<MaintenanceRecord> MaintenanceRecords { get; set; }
        public DbSet<Assignment> Assignments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Vehicle - VehicleType (many-to-one)
            modelBuilder.Entity<Vehicle>()
                .HasOne(v => v.VehicleType)
                .WithMany(vt => vt.Vehicles)
                .HasForeignKey(v => v.VehicleTypeId);

            // MaintenanceRecord - Vehicle (many-to-one)
            modelBuilder.Entity<MaintenanceRecord>()
                .HasOne(m => m.Vehicle)
                .WithMany(v => v.MaintenanceRecords)
                .HasForeignKey(m => m.VehicleId);

            // Assignment - Vehicle (many-to-one)
            modelBuilder.Entity<Assignment>()
                .HasOne(a => a.Vehicle)
                .WithMany(v => v.Assignments)
                .HasForeignKey(a => a.VehicleId);

            // Assignment - Driver (many-to-one)
            modelBuilder.Entity<Assignment>()
                .HasOne(a => a.Driver)
                .WithMany(d => d.Assignments)
                .HasForeignKey(a => a.DriverId);

            // Конфігурація для поля Cost
            modelBuilder.Entity<MaintenanceRecord>()
                .Property(m => m.Cost)
                .HasColumnType("decimal(10,2)");
        }
    }
}