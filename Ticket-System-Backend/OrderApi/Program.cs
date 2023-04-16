using Microsoft.EntityFrameworkCore;
using OrderApi.Data;
using OrderApi.Data.DbInitializer;
using OrderApi.Data.Repository;
using OrderApi.Infrastructure;
using SharedModels;

string AMQPConnectionString =
    "host=sparrow.rmq.cloudamqp.com;virtualHost=fealjkuy;username=fealjkuy;password=X7R3PC-9txrCgLqBqof9qltyOwHnZ3xU";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container

builder.Services.AddDbContext<OrderApiContext>(opt => opt.UseInMemoryDatabase("OrdersDb"));

// Register repositories for dependency injection
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

// Register database initializer for dependency injection
builder.Services.AddTransient<IDbInitializer, DbInitializer>();

builder.Services.AddSingleton<IMessagePublisher>(new
                MessagePublisher(AMQPConnectionString));

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
    var dbContext = services.GetService<OrderApiContext>();
    var dbInitializer = services.GetService<IDbInitializer>();
    dbInitializer.Initialize(dbContext);
}

// Create a message listener in a separate thread.
Task.Factory.StartNew(() =>
    new MessageListener(app.Services, AMQPConnectionString).Start());

app.UseHttpsRedirection();

app.UseAuthentication();

app.MapControllers();

app.Run();
