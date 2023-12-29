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

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

#region IOC

builder.Services.AddScoped<IGuestManager, GuestManager>();
builder.Services.AddScoped<IGuestRepository, GuestRepository>();
builder.Services.AddScoped<IRoomManager, RoomManager>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IBookingManager, BookingManager>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();

#endregion

#region Db Config

var connectionString = builder.Configuration.GetConnectionString("Main");
builder.Services.AddDbContext<HotelDbContext>(
    options => options.UseNpgsql(connectionString));

#endregion

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
