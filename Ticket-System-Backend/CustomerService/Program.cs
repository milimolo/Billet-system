using CustomerService.Data;
using CustomerService.Data.DbInitializer;
using CustomerService.Data.Repository;
using CustomerService.Infrastructure;
using CustomerService.Models;
using Microsoft.EntityFrameworkCore;

string AMQPConnectionString =
    "host=sparrow.rmq.cloudamqp.com;virtualHost=fealjkuy;username=fealjkuy;password=X7R3PC-9txrCgLqBqof9qltyOwHnZ3xU";

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

// Create a message listener in a separate thread.
Task.Factory.StartNew(() =>
    new MessageListener(app.Services, AMQPConnectionString).Start());

//app.UseHttpsRedirection();

app.UseAuthentication();

app.MapControllers();

app.Run();
