﻿using CustomerApi.Data;
using CustomerApi.Data.DbInitializer;
using CustomerApi.Data.Helpers;
using CustomerApi.Data.Repository;
using CustomerApi.Models;
using CustomerApi.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

string AMQPConnectionString =
    "host=sparrow.rmq.cloudamqp.com;virtualHost=fealjkuy;username=fealjkuy;password=X7R3PC-9txrCgLqBqof9qltyOwHnZ3xU";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container

Byte[] secretBytes = new byte[40];
Random rand = new Random();
rand.NextBytes(secretBytes);

builder.Services.AddAuthentication(Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = false,
        ValidateIssuer = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(secretBytes),
        ValidateLifetime = true, //validate the expiration and not before values in the token
        ClockSkew = TimeSpan.FromMinutes(5) //5 minute tolerance for the expiration date
    };
});

builder.Services.AddDbContext<CustomerApiContext>(opt => opt.UseInMemoryDatabase("CustomersDb"));

// Register repositories for dependency injection
builder.Services.AddScoped<IRepository<Customer>, CustomerRepository>();

// Register database initializer for dependency injection
builder.Services.AddTransient<IDbInitializer, DbInitializer>();

builder.Services.AddSingleton<IAuthenticationHelper>(new AuthenticationHelper(secretBytes));

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
    var dbContext = services.GetService<CustomerApiContext>();
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
