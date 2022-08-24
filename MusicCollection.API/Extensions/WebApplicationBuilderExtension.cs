using Microsoft.AspNetCore.HttpLogging;
using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace MusicCollection.API.Extensions;

public static class WebApplicationBuilderExtension
{
    public static void ConfigureLogging(this WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Information()
        .WriteTo.Console()
        .WriteTo.File($"logs/log_{DateTime.UtcNow.Date.ToString("yyyy_MM_dd")}.txt")
        .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri($"http://{builder.Configuration["ElasticSearch:Host"]}:{builder.Configuration["ElasticSearch:Port"]}"))
        {
            IndexFormat = $"Music_Collection_API_Logs",
            AutoRegisterTemplate = true,
            AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7
        })
        .CreateLogger();

        builder.Host.UseSerilog();

        builder.Services.AddHttpLogging(logging =>
        {
            logging.LoggingFields = HttpLoggingFields.RequestQuery
                | HttpLoggingFields.RequestBody
                | HttpLoggingFields.ResponseBody
                | HttpLoggingFields.Response;
            logging.RequestBodyLogLimit = 4096;
            logging.ResponseBodyLogLimit = 4096;
        });
    }

    public static void ConfigureAuth(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultScheme = "MusicCollection";
            options.DefaultAuthenticateScheme = "MusicCollection";
            options.DefaultChallengeScheme = "MusicCollection";
        }).AddCookie("MusicCollection", options =>
        {
            options.Cookie.Name = "MusicCollection.Cookie";
            options.Events.OnRedirectToLogin = context =>
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Task.CompletedTask;
            };
        });

        builder.Services.AddAuthorization();
    }

    public static void ConfigureDependencyInjections(this WebApplicationBuilder builder)
    {
        builder.Services.AddHttpContextAccessor();
    }
}