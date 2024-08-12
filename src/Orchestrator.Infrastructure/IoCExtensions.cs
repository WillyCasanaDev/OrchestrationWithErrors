using Hangfire.Dashboard.BasicAuthorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Orchestrator.Core.Interfaces;
using Orchestrator.CrossCutting.Exceptions;
using Orchestrator.Infrastructure.Extensions;
using Orchestrator.Infrastructure.Options;
using Orchestrator.Infrastructure.Providers;
using SharedUtilLibrary;

namespace Orchestrator.Infrastructure;

public static class IoCExtensions
{
    public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddVersionHealthCheck();
        services.AddScoped<ISecretProvider, VaultSecretProvider>();
        services.AddScoped<IRecurringJobScheduler, RecurringJobSchedulerService>();

        GetOptionsFromSettings(services, configuration);


        var provider = services.BuildServiceProvider();
        var dbOptions = provider.GetService<CosmosDbOptions>();
        var hangfireConfigurationOptions = provider.GetService<HangfireConfigurationOptions>();

        services.AddMongoRepository(dbOptions!.ConnectionString, dbOptions!.DatabaseName);
        //Add Task Manager
        services.AddTaskManager(dbOptions!.ConnectionString, dbOptions!.DatabaseName, hangfireConfigurationOptions.Queues);


        return services;
    }

    public static WebApplication UseHangfireDashboard(this WebApplication app, IServiceCollection services)
    {
        var provider = services.BuildServiceProvider();
        var hangfireConfigurationOptions = provider.GetService<HangfireConfigurationOptions>();

        app.UseTaskManagerDashboard(
            [
                new BasicAuthAuthorizationFilter(new BasicAuthAuthorizationFilterOptions()
                    {
                        RequireSsl = false,
                        SslRedirect = false,
                        LoginCaseSensitive = true,
                        Users =
                        [
                            new BasicAuthAuthorizationUser
                            {
                                Login = hangfireConfigurationOptions.Dashboard.User,
                                PasswordClear = hangfireConfigurationOptions.Dashboard.Password
                            }
                        ]
                    }
                    )
            ], hangfireConfigurationOptions.Dashboard.IsReadOnly);

        return app;
    }

    private static void GetOptionsFromSettings(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<VaultOptions>(configuration.GetSection("Vault"));
        var appSettingOptions = GetAppSettingsFromSecretProvider(services);

        configuration = new ConfigurationBuilder().AddConfiguration(configuration)
            .AddInMemoryCollection(appSettingOptions.ToDictionary(kvp => kvp.Key,
                kvp => kvp.Value.ToString()
            )).Build();

        if (configuration.TryGetSection<List<AzureAdB2COptions>>("AzureAdB2C", out var azureAdB2COptions) &&
            azureAdB2COptions != null)
            services.AddSingleton(azureAdB2COptions);
        else
            throw new InfrastructureException($"AzureAdB2C setting not found");


        if (configuration.TryGetSection<CosmosDbOptions>("CosmosDb", out var cosmosDbValue) && cosmosDbValue != null)
            services.AddSingleton(cosmosDbValue!);
        else
            throw new InfrastructureException("CosmosDB setting not found");


        if (configuration.TryGetSection<EnvironmentSetupOptions>("EnvironmentSetup", out var environmentSetupValue) &&
            environmentSetupValue != null)
            services.AddSingleton(environmentSetupValue!);
        else
            throw new InfrastructureException($"EnvironmentSetup setting not found");


        if (configuration.TryGetSection<EventGridOptions>("EventGrid", out var eventGridValue) &&
            eventGridValue != null)
            services.AddSingleton(eventGridValue!);
        else
            throw new InfrastructureException($"EventGrid setting not found");


        if (configuration.TryGetSection<RetryPolicyOptions>("RetryPolicy", out var retryPolicyValue) &&
            retryPolicyValue != null)
            services.AddSingleton(retryPolicyValue!);
        else
            throw new InfrastructureException($"RetryPolicy setting not found");


        if (configuration.TryGetSection<SendGridMulesoftOptions>("SendGridMulesoft", out var sendGridMulesoftValue) &&
            sendGridMulesoftValue != null)
            services.AddSingleton(sendGridMulesoftValue!);
        else
            throw new InfrastructureException($"SendGridMulesoft setting not found");



        if (configuration.TryGetSection<HangfireConfigurationOptions>("HangfireConfiguration", out var hangfireConfigurationValue) &&
           hangfireConfigurationValue != null)
            services.AddSingleton(hangfireConfigurationValue!);
        else
            throw new InfrastructureException($"HangfireConfiguration setting not found");

    }

    private static IDictionary<string, object> GetAppSettingsFromSecretProvider(IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var secretProvider = serviceProvider.GetRequiredService<ISecretProvider>();
        return secretProvider.GetSecret().Result;
    }

    private static bool TryGetSection<T>(this IConfiguration configuration, string sectionName, out T? option)
    {
        var section = configuration.GetSection(sectionName);
        option = section.Value is null ? section.Get<T>() : JsonConvert.DeserializeObject<T>(section.Value);
        return section.Exists();
    }
}