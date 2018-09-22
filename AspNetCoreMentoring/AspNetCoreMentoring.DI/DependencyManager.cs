using System;
using AspNetCoreMentoring.Core.Interfaces;
using AspNetCoreMentoring.Core.Services;
using AspNetCoreMentoring.Infrastructure;
using AspNetCoreMentoring.Infrastructure.EfEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCoreMentoring.DI
{
    public static class DependencyManager
    {
        public static void InstallApplicationDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IProductsService, ProductsService>();

            services.AddScoped<ICategoriesService, CategoriesService>();
            services.AddScoped<ISupplierService, SuppliersService>();
        }

        public static void InstallInfrastractureDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<NorthwindContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("NorthwindConnection")));

            services.AddScoped(typeof(IGenericRepository<>), typeof(EfGenericRepository<>));

            //services.AddTransient(typeof(ILogger<>), typeof(FileLogger<>));
        }

        //public static void InstallApplicationDependencies(this IServiceCollection services, IConfiguration configuration)
        //{
        //    services.AddPersistence(configuration);
        //    services.AddMessaging(configuration);
        //    services.AddNotification(configuration);
        //    services.AddLogging(configuration);

        //    services.AddScoped<ISystemUserService, SystemUserService>();
        //    services.AddScoped<ICustomerService, CustomerService>();
        //}

        //private static void AddNotification(this IServiceCollection services, IConfiguration configuration)
        //{
        //    services.AddSingleton(sp =>
        //    {
        //        var emailServerConnectionSettings = new EmailServerConnectionSettings
        //        {
        //            ApiKey = configuration["SendGridApiKey"],
        //            SenderEmailAddress = configuration["SendGridSenderEmail"],
        //            SenderName = configuration["SendGridSenderName"]
        //        };

        //        return emailServerConnectionSettings;
        //    });

        //    services.AddTransient<IEmailHtmlContentBuilder, EmailHtmlContentBuilder>();
        //    services.AddTransient<IEmailBuilder<UserInvitationEmailModel>, UserInvitationEmailBuilder>();

        //    services.AddTransient<INotificationService, NotificationService>((serviceProvider) =>
        //    {
        //        var emailConnectionSettings = serviceProvider.GetRequiredService<EmailServerConnectionSettings>();
        //        var emailFactory = new EmailClientFactory();
        //        var sendGridClient = emailFactory.CreateClient(emailConnectionSettings.ApiKey);
        //        var emailBuilderRetriever = new EmailBuilderRetriever(serviceProvider, emailConnectionSettings);
        //        return new NotificationService(sendGridClient, emailBuilderRetriever);
        //    });
        //}

        //private static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
        //{
        //    services.AddSingleton(sp =>
        //    {
        //        var connectionSettings = GetCosmosDbConnectingSettings(configuration, configuration["CosmosDbSystemUserCollectionName"]);
        //        return connectionSettings;
        //    });

        //    services.AddSingleton<ICosmosDbClientFactory, DefaultCosmosDbClientFactory>();

        //    services.AddSingleton(sp =>
        //    {
        //        var connectionSettings = sp.GetRequiredService<CosmosDbConnectionSettings>();
        //        var jsonSettings = JsonConvertExtensions.CreateDefaultSerializerSettings();

        //        var factory = sp.GetRequiredService<ICosmosDbClientFactory>();

        //        return factory.CreateClient(connectionSettings, jsonSettings);
        //    });

        //    services.AddScoped<ISystemUserRepository, SystemUserRepository>(x =>
        //        new SystemUserRepository(new CosmosDbDocumentStorage(
        //            x.GetRequiredService<DocumentClient>(),
        //            GetCosmosDbConnectingSettings(configuration, configuration["CosmosDbSystemUserCollectionName"]))));

        //    services.AddScoped<ICustomerRepository, CustomerRepository>(x =>
        //        new CustomerRepository(new CosmosDbDocumentStorage(
        //            x.GetRequiredService<DocumentClient>(),
        //            GetCosmosDbConnectingSettings(configuration, configuration["CosmosDbCustomerCollectionName"]))));
        //}

        //private static void AddMessaging(this IServiceCollection services, IConfiguration configuration)
        //{
        //    services.AddScoped<ISenderClient>(p =>
        //    {
        //        var connectionStringBuilder = new ServiceBusConnectionStringBuilder(configuration["UserManagementServiceBusConnectionString"]);

        //        return new TopicClient(connectionStringBuilder);
        //    });

        //    services.AddScoped<IDomainEventConverter, DefaultDomainEventConverter>();
        //    services.AddScoped<IDomainEventBus, ServiceBusDomainEventSender>();
        //}

        //private static void AddLogging(this IServiceCollection services, IConfiguration configuration)
        //{
        //    services.AddScoped<IContextLogger, ContextLogger>();

        //    var provider = services.BuildServiceProvider();

        //    Log.Logger = new LoggerConfiguration()
        //        .Enrich.With(provider.GetService<IContextLogger>())
        //        .WriteTo
        //        .ApplicationInsightsTraces(configuration["ApplicationInsightsInstrumentationKey"])
        //        .CreateLogger();

        //    services.AddSingleton(Log.Logger);
        //}

        //private static CosmosDbConnectionSettings GetCosmosDbConnectingSettings(IConfiguration configuration, string collectionName)
        //{
        //    var settings = new CosmosDbConnectionSettings
        //    {
        //        AuthKey = configuration["CosmosDbAuthKey"],
        //        Endpoint = configuration["CosmosDbEndpoint"],
        //        DatabaseName = configuration["CosmosDbDatabaseName"],
        //        CollectionName = collectionName,
        //        ConnectionPolicy = new ConnectionPolicy()
        //    };

        //    return settings;
        //}
    }
}
