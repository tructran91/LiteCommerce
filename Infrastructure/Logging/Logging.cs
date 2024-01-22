using Microsoft.Extensions.Hosting;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using Serilog;
using Serilog.Exceptions;
using Microsoft.Extensions.Configuration;

namespace Common.Logging
{
    public static class Logging
    {
        public static Action<HostBuilderContext, LoggerConfiguration> ConfigureLogger =>
        (context, loggerConfiguration) =>
        {
            var env = context.HostingEnvironment;
            loggerConfiguration.MinimumLevel.Information()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("ApplicationName", env.ApplicationName)
                .Enrich.WithProperty("EnvironmentName", env.EnvironmentName)
                .Enrich.WithExceptionDetails()
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                .WriteTo.Console();
            if (context.HostingEnvironment.IsDevelopment())
            {
                loggerConfiguration.MinimumLevel.Override("Catalog", LogEventLevel.Information);
                //loggerConfiguration.MinimumLevel.Override("Basket", LogEventLevel.Debug);
                //loggerConfiguration.MinimumLevel.Override("Discount", LogEventLevel.Debug);
                //loggerConfiguration.MinimumLevel.Override("Ordering", LogEventLevel.Debug);
            }

            //var elasticConfiguration = context.Configuration.GetSection(nameof(ElasticConfiguration)).Get<ElasticConfiguration>();
            //if (!string.IsNullOrEmpty(elasticConfiguration.Uri))
            //{
            //    loggerConfiguration
            //        .Enrich.FromLogContext()
            //        .Enrich.WithProperty("ApplicationName", env.ApplicationName)
            //        .Enrich.WithProperty("EnvironmentName", env.EnvironmentName)
            //        .WriteTo.Elasticsearch(
            //        new ElasticsearchSinkOptions(new Uri(elasticConfiguration.Uri))
            //        {
            //            AutoRegisterTemplate = true,
            //            AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv8,
            //            IndexFormat = "LiteCommerce-Logs-{0:yyyy.MM.dd}",
            //            MinimumLogEventLevel = LogEventLevel.Information
            //        });
            //}
        };
    }
}
