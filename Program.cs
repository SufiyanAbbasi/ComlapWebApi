using ComlapWebApi.Models;
using ComlapWebApi.Models.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<ComlapDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ComlapDbConnection"));
});


//cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200") // Add your Angular application's origin
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});
//for authentication and authorization
//builder.Services.AddIdentity<DbContext, User>()
//    .AddEntityFrameworkStores<ComlapDbContext>();
//builder.Services.AddAuthorization();



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

app.UseCors("AllowSpecificOrigin");

app.UseAuthorization();

app.MapControllers();

app.Run();
