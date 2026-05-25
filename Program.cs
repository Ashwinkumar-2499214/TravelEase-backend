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
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IEmailService, EmailService>();

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
