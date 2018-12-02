using System;
using System.IO;
using AspNetCoreMentoring.DI;
using AspNetCoreMentoring.Notification;
using AspNetCoreMentoring.Notification.EmailBuilders;
using AspNetCoreMentoring.Notification.EmailClient;
using AspNetCoreMentoring.Notification.EmailTemplates;
using AspNetCoreMentoring.Notification.Models;
using AspNetCoreMentoring.UI.Filters;
using AspNetCoreMentoring.UI.Middleware;
using AspNetCoreMentoring.UI.Models;
using AspNetCoreMentoring.UI.ViewComponents;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace AspNetCoreMentoring.UI
{
    public class Startup
    {
        public Startup(IConfiguration configuration, ILogger<Startup> eventLogger, ILoggerFactory loggerFactory)
        {
            Configuration = configuration;
            Logger = eventLogger;
            LoggerFactory = loggerFactory;
        }

        public IConfiguration Configuration { get; }
        public ILogger<Startup> Logger { get; }
        public ILoggerFactory LoggerFactory { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            Logger.LogInformation("Current Connection string {0}", Configuration.GetConnectionString("NorthwindConnection"));

            services.InstallInfrastractureDependencies(Configuration);
            services.InstallApplicationDependencies(Configuration);
            InstallIdentityDependencies(services, Configuration);

            services.AddSingleton<IImageCacheHelper, ImageCacheHelper>();

            services.AddAutoMapper();

            services.AddMvc(options =>
            {
                options.Filters.Add(new ActionExecutionLoggerFilter(LoggerFactory)
                {
                    LogOnStart = true,
                    LogOnStop = true
                });
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddTransient<BreadcrumbService>();

            InstallNotificationDependencies(services);
        }

        private void InstallNotificationDependencies(IServiceCollection services)
        {
            services.AddTransient<EmailBuilder<UserActivationEmailModel>, UserActivationEmailBuilder>();
            services.AddTransient<EmailBuilder<ResetPasswordEmailModel>, ForgotPasswordEmailBuilder>();

            services.AddSingleton(sp =>
            {
                var emailServerConnectionSettings = new EmailServerConnectionSettings
                {
                    ApiKey = Configuration["SendGridApiKey"],
                    SenderEmailAddress = Configuration["SendGridSenderEmail"],
                    SenderName = Configuration["SendGridSenderName"]
                };

                return emailServerConnectionSettings;
            });

            services.AddTransient<INotificationService, NotificationService>((serviceProvider) =>
            {
                var emailConnectionSettings = serviceProvider.GetRequiredService<EmailServerConnectionSettings>();
                var emailFactory = new EmailClientFactory();
                var sendGridClient = emailFactory.CreateClient(emailConnectionSettings.ApiKey);
                var emailBuilderRetriever = new EmailBuilderRetriever(serviceProvider, emailConnectionSettings);
                return new NotificationService(sendGridClient, emailBuilderRetriever);
            });


            //var assembly = typeof(INotificationService).GetTypeInfo().Assembly;
            //var embeddedFileProvider = new EmbeddedFileProvider(
            //    assembly,
            //    "ViewComponentLibrary"
            //);

            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.FileProviders.Clear();
                options.FileProviders.Add(new PhysicalFileProvider(Directory.GetCurrentDirectory()));
            });

            services.AddTransient<IRazorViewToStringRenderer, RazorViewToStringRenderer>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
        IApplicationBuilder app,
        IHostingEnvironment env,
        IApplicationLifetime applicationLifetime,
        ILogger<Startup> eventLogger,
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }


            app.UseStatusCodePagesWithReExecute("/error/{0}");

            applicationLifetime.ApplicationStarted.Register(() =>
                eventLogger.LogInformation("Custom Log AppStarted with path {0}", env.ContentRootPath));
            applicationLifetime.ApplicationStopping.Register(() =>
                eventLogger.LogInformation("Custom Log AppStopped"));

            app.UseStaticFiles();

            app.UseFileServer(new FileServerOptions()
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(env.ContentRootPath, "node_modules")
                ),
                RequestPath = "/node_modules",
                EnableDirectoryBrowsing = false
            });

            app.UseAuthentication();
            MyIdentityDataInitializer.SeedData(userManager, roleManager);

            app.UseImageCachingMiddleware(new CachingOptions
            {
                CacheDirectory = "CachedImages",
                CacheDuration = new System.TimeSpan(0, 0, 60),
                MaxImages = 3
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        public void InstallIdentityDependencies(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AspNetIdentityContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("AspNetUsersDb")));

            services.AddMvc()
             .AddRazorPagesOptions(options =>
             {
                 options.AllowAreas = true;
                 options.Conventions.AuthorizeAreaFolder("Identity", "/Account/Manage");
                 options.Conventions.AuthorizeAreaPage("Identity", "/Account/Logout");
             });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
            });

            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 2;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

            })
            //.AddRoles<IdentityRole>()
            .AddRoleManager<RoleManager<IdentityRole>>()
            .AddDefaultUI()
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<AspNetIdentityContext>();


            services.AddAuthentication()
                .AddOpenIdConnect(AzureADDefaults.AuthenticationScheme, "EPAM Azure AD", opts =>
            {
                Configuration.Bind("AzureAd", opts);
                opts.Authority = $"{Configuration["AzureAd:Instance"]}{Configuration["AzureAd:TenantId"]}/v2.0/";
                opts.Scope.Add("email");
                opts.Scope.Add("openid");
                opts.Scope.Add("profile");
                opts.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "preffered_name"
                };
            });
        }
    }
}

