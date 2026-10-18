using Inventory.Api.Filters;
using Inventory.Application;
using Inventory.Infrastructure;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// --- Services ---
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
});

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// Application (services, DTOs, interfaces)
builder.Services.AddApplication();

// Infrastructure (DbContext, repositories)
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// --- Middleware pipeline ---
if (app.Environment.IsDevelopment())
{
    // Swagger/OpenAPI will be added later as a schema-only endpoint.
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
