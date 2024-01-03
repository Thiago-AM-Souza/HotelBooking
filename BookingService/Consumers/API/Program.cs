using Microsoft.EntityFrameworkCore;
using Data;
using Application.Guest.Ports;
using Application;
using Data.Guest;
using Domain.Guest.Ports;
using Application.Room.Ports;
using Application.Room;
using Domain.Room.Ports;
using Data.Room;
using Application.Booking;
using Data.Booking;
using Domain.Booking.Ports;
using Application.Booking.Ports;
using Application.Payment;
using Application.MercadoPago;
using Application.Payment.Ports;
using Payments.Application;
using System.Text.Json.Serialization;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Application.Booking.Commands;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining(typeof(Program));
    cfg.RegisterServicesFromAssemblyContaining(typeof(CreateBookingCommand));
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
