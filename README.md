# Dotnet.Cep

Project to search  for Brazilian CEPs in various providers and return the fastest result.


## Technical Goals

- [x] Add Telemetry
  - [x] Send Logs to Standalone Aspire Dashboard
  - [ ] Send Logs to New Relic
- [x] Experiment `Microsoft.Extensions.Resilience.Http`
- [x] Experiment `Refit`
- [x] Experiment `Microsoft.Extension.HybridCache`
- [x] Experiment `Scalar`
- [x] Experiment `HybridCache`
  - The tracing is lsot when using the HybridCache, but I didn't find a way to solve this by now
- [x] Docker
