using Inventory.Application.Interfaces;
using Inventory.Application.Services;
using Inventory.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// --- Services ---
builder.Services.AddControllers();

// Infrastructure (DbContext, repositories)
builder.Services.AddInfrastructure(builder.Configuration);

// Application services
builder.Services.AddScoped<IProductService, ProductService>();

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
