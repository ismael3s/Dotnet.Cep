using Dotnet.Cep.Core.Services;
using Dotnet.Cep.Infra;
using Microsoft.AspNetCore.Mvc;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) =>
    loggerConfig.ReadFrom.Configuration(context.Configuration));

builder.Services.AddOpenApi();
builder.Services.AddDependencies(builder.Configuration);
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.MapGet("/cep/{cep:required:minlength(8):maxlength(8)}", async (string cep,
        CancellationToken requestCancellationToken,
        [FromServices] CepService cepService
    ) => await cepService.GetCepAsync(cep, requestCancellationToken))
    .WithName("CEP");

app.Run();