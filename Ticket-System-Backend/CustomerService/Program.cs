using CustomerService.Data;
using CustomerService.Data.DbInitializer;
using CustomerService.Data.Repository;
using CustomerService.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

string AMQPConnectionString =
    "host=roedeer.rmq.cloudamqp.com;virtualHost=hmkzgqhj;username=hmkzgqhj;password=TbxxIbE4-PwgOS2KbToo7aSJdV8H3XsJ";

// Add services to the container

builder.Services.AddDbContext<CustomerServiceContext>(opt => opt.UseInMemoryDatabase("CustomersDb"));

// Register repositories for dependency injection
builder.Services.AddScoped<IRepository<Customer>, CustomerRepository>();

// Register database initializer for dependency injection
builder.Services.AddTransient<IDbInitializer, DbInitializer>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetService<CustomerServiceContext>();
    var dbInitializer = services.GetService<IDbInitializer>();
    dbInitializer.Initialize(dbContext);
}

app.UseHttpsRedirection();

app.Run();
