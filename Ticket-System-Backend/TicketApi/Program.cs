using Microsoft.EntityFrameworkCore;
using TicketApi.Data;
using TicketApi.Data.DbInitializer;
using TicketApi.Data.Repository;
using TicketApi.Infrastructure;
using TicketApi.Models;

string AMQPConnectionString =
    "host=sparrow.rmq.cloudamqp.com;virtualHost=fealjkuy;username=fealjkuy;password=X7R3PC-9txrCgLqBqof9qltyOwHnZ3xU";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container

builder.Services.AddDbContext<TicketApiContext>(opt => opt.UseInMemoryDatabase("OrdersDb"));

// Register repositories for dependency injection
builder.Services.AddScoped<IRepository<Ticket>, TicketRepository>();

// Register database initializer for dependency injection
builder.Services.AddTransient<IDbInitializer, DbInitializer>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(policyBuilder =>
    policyBuilder.AddDefaultPolicy(policy =>
        policy.WithOrigins("*").AllowAnyHeader().AllowAnyHeader())
);

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
    var dbContext = services.GetService<TicketApiContext>();
    var dbInitializer = services.GetService<IDbInitializer>();
    dbInitializer.Initialize(dbContext);
}

// Create a message listener in a separate thread.
Task.Factory.StartNew(() =>
    new MessageListener(app.Services, AMQPConnectionString).Start());

//app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();

app.MapControllers();

app.Run();