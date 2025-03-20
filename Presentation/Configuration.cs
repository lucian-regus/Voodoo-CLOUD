using System.Reflection;
using Domain.Database;
using Domain.Exceptions;
using Domain.Repositories;
using Infrastructure.Jobs;
using Infrastructure.Services;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Quartz;

namespace Presentation;

public static class Configuration
{
    public static IServiceCollection AddDatabaseContext(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        services.AddDbContext<DatabaseContext>(options =>
            options
                .UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
        );
        
        return services;
    }
    
    public static IApplicationBuilder UseExceptionHandlerMiddleware(this IApplicationBuilder app) => app.UseExceptionHandler(builder =>
        {
            builder.Run(async context =>
            {
                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                {
                    if (contextFeature.Error.InnerException is null)
                    {
                        context.Items["Exception"] = contextFeature.Error.Message;
                        context.Items["StackTrace"] = contextFeature.Error.StackTrace;
                    }
                    else
                    {
                        context.Items["Exception"] = $"{contextFeature.Error.Message}\n{contextFeature.Error.InnerException.Message}";
                        context.Items["StackTrace"] = $"{contextFeature.Error.StackTrace}\n{contextFeature.Error.InnerException.StackTrace}";
                    }

                    context.Response.StatusCode = contextFeature.Error switch
                    {
                        BadRequestException => StatusCodes.Status400BadRequest,
                        _ => StatusCodes.Status500InternalServerError
                    };

                    if (contextFeature.Error is AggregateException aggregateException && aggregateException.InnerExceptions.Any(e => e is BadRequestException))
                    {
                        await context.Response.WriteAsJsonAsync(new
                        {
                            message = string.Join("\n", aggregateException.InnerExceptions.Select(e => e.Message)),
                            timestamp = DateTime.UtcNow
                        });
                    }
                    else if (context.Response.StatusCode == StatusCodes.Status500InternalServerError)
                    {
                        await context.Response.WriteAsJsonAsync(new
                        {
                            message = "Internal Server Error",
                            details = contextFeature.Error.Message,
                            type = contextFeature.Error.GetType().Name,
                            timestamp = DateTime.UtcNow
                        });
                    }
                    else
                    {
                        await context.Response.WriteAsJsonAsync(new
                        {
                            message = contextFeature.Error.Message,
                            timestamp = DateTime.UtcNow
                        });
                    }
                }
            });
        });  
    
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services
            .AddScoped<IBlacklistedIpAddressRepository, BlacklistedIpAddressRepository>()
            .AddScoped<IMalwareSignatureRepository, MalwareSignatureRepository>()
            .AddScoped<IYaraRuleRepository, YaraRuleRepository>()
            .AddScoped<IScrapingLogRepository, ScrapingLogRepository>();
        
        return services;
    }
    
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services
            .AddScoped<IUnitOfWork, UnitOfWork>()
            .AddScoped<IServiceBase, ServiceBase>()
            .AddScoped<IDatabaseService, DatabaseService>()
            .AddScoped<IThreatsService, ThreatsService>();
        
        return services;
    }
    
    public static IServiceCollection AddSwagger(this IServiceCollection services) => services.AddSwaggerGen(options =>
    {
        var assemblyDate = File.GetLastWriteTimeUtc(Assembly.GetExecutingAssembly().Location);
        var buildNo = assemblyDate.DayOfYear + "." + (assemblyDate.Hour * 60 + assemblyDate.Minute);
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Voodoo",
            Version = $"Build {buildNo} on date " + assemblyDate.ToString("dd/MM hh:mm")
        });

        options.CustomSchemaIds(type => type.FullName);

    }).AddSwaggerGenNewtonsoftSupport();

    public static IServiceCollection AddQuartzJobs(this IServiceCollection services)
    {
        services
            .AddQuartz(quartz =>
            {
                var jobKey = new JobKey("IpSumJob");
                quartz.AddJob<IpSumJob>(opts => opts.WithIdentity(jobKey));

                quartz.AddTrigger(opts => opts
                    .ForJob(jobKey)
                    .StartAt(DateBuilder.FutureDate(1, IntervalUnit.Hour))
                    .WithSimpleSchedule( s => s
                        .WithIntervalInHours(1)
                        .RepeatForever()
                    )
                );
            });
            
        return services;
    }

    public static IServiceCollection AddHttpClients(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddHttpClient<IpSumJob>(client =>
            {
                client.BaseAddress = new Uri(configuration["IpSumTarget:BaseURL"]);
            });

        return services;
    }
}