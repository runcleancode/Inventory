using Inventory.Application;
using Inventory.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// --- Services ---
builder.Services.AddControllers();

// Application (services, DTOs, interfaces)
builder.Services.AddApplication();

// Infrastructure (DbContext, repositories)
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// --- Middleware pipeline ---
if (app.Environment.IsDevelopment())
{
    // Swagger/OpenAPI will be added later as a schema-only endpoint.
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
