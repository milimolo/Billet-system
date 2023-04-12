using CustomerService.Data;
using CustomerService.Data.DbInitializer;
using CustomerService.Data.Repository;
using CustomerService.Models;
using Microsoft.EntityFrameworkCore;

string AMQPConnectionString =
    "host=roedeer.rmq.cloudamqp.com;virtualHost=hmkzgqhj;username=hmkzgqhj;password=TbxxIbE4-PwgOS2KbToo7aSJdV8H3XsJ";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container

builder.Services.AddDbContext<CustomerServiceContext>(opt => opt.UseInMemoryDatabase("CustomersDb"));

// Register repositories for dependency injection
builder.Services.AddScoped<IRepository<Customer>, CustomerRepository>();

// Register database initializer for dependency injection
builder.Services.AddTransient<IDbInitializer, DbInitializer>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Initialize the database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetService<CustomerServiceContext>();
    var dbInitializer = services.GetService<IDbInitializer>();
    dbInitializer.Initialize(dbContext);
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.MapControllers();

app.Run();
