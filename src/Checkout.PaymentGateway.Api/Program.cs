using Checkout.PaymentGateway.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

Bootstrapper.Bootstrap(builder.Services);

builder.Services.AddControllers();
builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseMiddleware<ErrorHandlerMiddleware>();
app.MapControllers();
app.UseHealthChecks("/healthcheck");

app.Run();