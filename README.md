# Dotnet.Cep

Project to search  for Brazilian CEPs in various providers and return the fastest result.


## Technical Goals

- [x] Add Telemetry
  - [x] Send Logs to Standalone Aspire Dashboard
  - [x] Send Logs to New Relic
    ```json
{
  "OTEL_EXPORTER_OTLP_ENDPOINT": "https://otlp.nr-data.net",
  "OTEL_EXPORTER_OTLP_HEADERS": "api-key=api-key",
  "OTEL_SERVICE_NAME": "Dotnet.Cep"
}
    ```
- [x] Experiment `Microsoft.Extensions.Resilience.Http`
- [x] Experiment `Refit`
- [x] Experiment `Microsoft.Extension.HybridCache`
- [x] Experiment `Scalar`
- [x] Experiment `HybridCache`
  - The tracing is lsot when using the HybridCache, but I didn't find a way to solve this by now
- [x] Docker
