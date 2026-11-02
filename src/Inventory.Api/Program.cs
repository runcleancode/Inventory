using Inventory.Api.ExceptionHandlers;
using Inventory.Application;
using Inventory.Infrastructure;
using Serilog;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

var builder = WebApplication.CreateBuilder(args);

// --- Serilog ---
builder.Host.UseSerilog((context, loggerConfiguration) =>
{
    string seqUrl = context.Configuration["Serilog:SeqUrl"]
        ?? throw new InvalidOperationException("Serilog:SeqUrl configuration is missing.");

    loggerConfiguration
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext()
        .Enrich.WithMachineName()
        .Enrich.WithEnvironmentName()
        .WriteTo.Console(
            outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
        .WriteTo.Seq(seqUrl);
});

// --- Services ---
builder.Services.AddControllers();

// FluentValidation auto-validation (replaces manual ValidationFilter)
builder.Services.AddFluentValidationAutoValidation();

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

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();
app.MapControllers();

app.Run();
