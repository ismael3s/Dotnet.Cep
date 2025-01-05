using Dotnet.Cep.Core.Services;
using Dotnet.Cep.Infra;
using Microsoft.AspNetCore.Mvc;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.ConfigureOpenTelemetry();
builder.Services.AddOpenApi();
builder.Services.AddDependencies(builder.Configuration);
var app = builder.Build();
app.MapOpenApi();
app.MapScalarApiReference();

app.UseHttpsRedirection();
app.MapGet("/v1/cep/{cep:required:minlength(8):maxlength(8)}", async (string cep,
        CancellationToken requestCancellationToken,
        [FromServices] CepService cepService
    ) => await cepService.GetCepAsync(cep, false, requestCancellationToken))
    .WithTags("Without Cache")
    .WithDescription("Tracing works like a charm in this endpoint")
    .WithName("CEP");

app.MapGet("/v2/cep/{cep:required:minlength(8):maxlength(8)}", async (string cep,
        CancellationToken requestCancellationToken,
        [FromServices] CepService cepService
    ) => await cepService.GetCepAsync(cep, true, requestCancellationToken))
    .WithTags("With Cache")
    .WithDescription("This endpoint wont trace the requests properly due a bug in the Hybrid Cache package.")
    .WithName("Cached Cep");

app.Run();