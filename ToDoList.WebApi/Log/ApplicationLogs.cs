
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

public static class ApplicationLogs
{
    private const string ServiceName = "To Do list";
    private const string ServiceVersion = "1.0.0";

    public static void AddOpenTelemetryTracing(this IServiceCollection services)
    {
        ResourceBuilder resourceBuilder = ResourceBuilder.CreateDefault().AddService(ServiceName, ServiceVersion);

        services.AddOpenTelemetry()
            .WithTracing(t => t
                .SetResourceBuilder(resourceBuilder)
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddSqlClientInstrumentation()
                .AddOtlpExporter(otlpOptions =>
                {
                    otlpOptions.Endpoint = new Uri("https://ingest.eu.signoz.cloud:443");
                    string headerKey = "signoz-ingestion-key";
                    string headerValue = "6deaa28e-6e01-46a7-bea4-b6144c08f615";

                    string formattedHeader = $"{headerKey}={headerValue}";
                    otlpOptions.Headers = formattedHeader;
                }))
            .WithMetrics(m => m
                .SetResourceBuilder(resourceBuilder)
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddSqlClientInstrumentation()
                .AddRuntimeInstrumentation()
                .AddOtlpExporter(otlpOptions =>
                {
                    otlpOptions.Endpoint = new Uri("https://ingest.eu.signoz.cloud:443");
                    otlpOptions.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
                }));
    }

    public static void AddOpenTelemetryLogging(this ILoggingBuilder logging)
    {
        ResourceBuilder resourceBuilder = ResourceBuilder.CreateDefault().AddService(ServiceName, ServiceVersion);

        logging.ClearProviders();
        logging.AddOpenTelemetry(loggingOptions =>
        {
            loggingOptions.SetResourceBuilder(resourceBuilder);
            loggingOptions.IncludeScopes = true;
            loggingOptions.IncludeFormattedMessage = true;
            loggingOptions.AddOtlpExporter(o =>
            {
                o.Endpoint = new Uri("https://ingest.eu.signoz.cloud:443");
            });
            loggingOptions.AddConsoleExporter();
        });
    }
}