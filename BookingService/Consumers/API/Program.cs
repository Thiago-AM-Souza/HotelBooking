using Application;
using Application.Booking;
using Application.Booking.Commands;
using Application.Booking.Ports;
using Application.Booking.Queries;
using Application.Guest.Ports;
using Application.MercadoPago;
using Application.Payment.Ports;
using Application.Room;
using Application.Room.Commands;
using Application.Room.Ports;
using Application.Room.Queries;
using Data;
using Data.Booking;
using Data.Guest;
using Data.Room;
using Domain.Booking.Ports;
using Domain.Guest.Ports;
using Domain.Room.Ports;
using Microsoft.EntityFrameworkCore;
using Payments.Application;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining(typeof(Program));
    cfg.RegisterServicesFromAssemblyContaining(typeof(CreateBookingCommand));
    cfg.RegisterServicesFromAssemblyContaining(typeof(GetBookingQuery));
    cfg.RegisterServicesFromAssemblyContaining(typeof(CreateRoomCommand));
    cfg.RegisterServicesFromAssemblyContaining(typeof(GetRoomQuery));
});

#region IOC

builder.Services.AddScoped<IGuestManager, GuestManager>();
builder.Services.AddScoped<IGuestRepository, GuestRepository>();
builder.Services.AddScoped<IRoomManager, RoomManager>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IBookingManager, BookingManager>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IPaymentProcessor, MercadoPagoAdapter>();
builder.Services.AddScoped<IPaymentProcessorFactory, PaymentProcessorFactory>();

#endregion

#region Db Config

var connectionString = builder.Configuration.GetConnectionString("Main");
builder.Services.AddDbContext<HotelDbContext>(
    options => options.UseNpgsql(connectionString));

#endregion

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CONVERTE ENUMS PARA APRESENTACAO NO SCHEMAS DO SWAGGER
builder.Services.AddControllersWithViews()
                .AddJsonOptions(options =>
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
