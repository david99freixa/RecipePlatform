using Microsoft.EntityFrameworkCore;
using RecipePlatform.Data;
using RecipePlatform.Data.Repositories;
using RecipePlatform.Domain.Interfaces;
using RecipePlatform.IoC;
using RecipePlatform.Services;
using RecipePlatform.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Registar repositories, services e a UnitOfWork com o IoC container
builder.Services.AddRecipePlatformServices(builder.Configuration);

// Registar os controllers
builder.Services.AddControllers();

builder.Services.AddOpenApi();

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
