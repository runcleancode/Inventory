using Inventory.Api.ExceptionHandlers;
using Inventory.Api.Filters;
using Inventory.Application;
using Inventory.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// ---Serilog---
builder.Host.UseSerilog((context, loggerConfiguration) =>
{
    loggerConfiguration
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext()
        .Enrich.WithMachineName()
        .Enrich.WithEnvironmentName()
        .WriteTo.Console(
            outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
        .WriteTo.Seq(context.Configuration["Serilog:SeqUrl"]!);
});

// --- Services ---
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
});

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// Problem Details (RFC 7807)
builder.Services.AddProblemDetails();

// Exception handling
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// Application (services, DTOs, interfaces)
builder.Services.AddApplication();

// Infrastructure (DbContext, repositories)
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// --- Middleware pipeline ---
app.UseSerilogRequestLogging();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    // Swagger/OpenAPI will be added later as a schema-only endpoint.
    // app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
