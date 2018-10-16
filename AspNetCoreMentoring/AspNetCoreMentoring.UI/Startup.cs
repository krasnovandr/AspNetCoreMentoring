using System.IO;
using AspNetCoreMentoring.DI;
using AspNetCoreMentoring.UI.Filters;
using AspNetCoreMentoring.UI.Middleware;
using AspNetCoreMentoring.UI.ViewComponents;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;

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

            services.AddSingleton<IImageCacheHelper, ImageCacheHelper>();

            services.AddAutoMapper();

            services.AddMvc(options =>
            {
                options.Filters.Add(new ActionExecutionLoggerFilter(LoggerFactory)
                {
                    LogOnStart = true,
                    LogOnStop = true
                });
            });

            services.AddTransient<BreadcrumbService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
        IApplicationBuilder app,
        IHostingEnvironment env,
        IApplicationLifetime applicationLifetime,
        ILogger<Startup> eventLogger)
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
    }
}
