using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using TravelEase.Model;
using TravelEaseBackend.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<BookingModel> Destinations { get; set; }
    public DbSet<ReservationModel> Bookings { get; set; }
    public DbSet<Notification> Notifications { get; set; }

    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<Payment> Payments { get; set; }

    public DbSet<KpiReport> KpiReports { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<KpiReport>(entity =>
    {
        entity.HasKey(e => e.ReportId);
        entity.Property(e => e.Title)
              .IsRequired()
              .HasMaxLength(200);
        entity.Property(e => e.GeneratedAt)
              .IsRequired();
        entity.Property(e => e.FilePath)
              .HasMaxLength(500);
    });
}
    
    
}