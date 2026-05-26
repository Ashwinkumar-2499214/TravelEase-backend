using Microsoft.EntityFrameworkCore;
using TravelEase.Model;

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
    
}