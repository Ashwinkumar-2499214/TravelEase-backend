using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Register application services and repositories
builder.Services.AddScoped<TravelEase.Service.Interface.IBookingService, TravelEase.Service.Implementation.BookingService>();
builder.Services.AddScoped<TravelEase.Repository.Interface.IBookingRepository, TravelEase.Repository.Implementation.BookingRepository>();
builder.Services.AddScoped<TravelEase.Repository.Interface.IReservationRepository, TravelEase.Repository.Implementation.ReservationRepository>();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,              // how many times to retry
            maxRetryDelay: TimeSpan.FromSeconds(30), // max delay between retries
            errorNumbersToAdd: null        // you can specify SQL error codes if needed
        )
    )
);

builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IEmailService, EmailService>();

// Register repositories
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();

// Register services
builder.Services.AddScoped<IInvoiceService, InvoiceService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
