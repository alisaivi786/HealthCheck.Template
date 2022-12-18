using Asp.Net.Core.HealthCheck.ConfigureServices;
using HealthCheck.Template.HealthServices;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Net;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCustomHealthChecks(configuration);

builder.Services.AddHealthChecks()
    .AddSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), tags: new[] { "Database" })
    .AddCheck<MyHealthCheck>("MyHealthCheck", tags: new[] { "Api Response" })
    .AddUrlGroup(new Uri("https://www.google.com"), name: "Google Service", tags: new[] { "Google Api" }, failureStatus: HealthStatus.Degraded)
    .AddUrlGroup(new Uri("https://www.stacknative.com"), name: "Stack Native Service", tags: new[] { "Stacknative Api" }, failureStatus: HealthStatus.Degraded)
    
    ;

builder.Services.AddHealthChecksUI()
    .AddInMemoryStorage();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHealthChecks("/health", new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
});

app.MapHealthChecksUI();
app.UseHttpsRedirection();


app.UseAuthorization();

app.MapControllers();

app.Run();
