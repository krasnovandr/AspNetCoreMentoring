using System.IO;
using AspNetCoreMentoring.DI;
using AspNetCoreMentoring.UI.Middleware;
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
        public Startup(IConfiguration configuration, ILogger<Startup> eventLogger)
        {
            Configuration = configuration;
            Logger = eventLogger;
        }

        public IConfiguration Configuration { get; }
        public ILogger<Startup> Logger { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            Logger.LogInformation("Current Connection string {0}", Configuration.GetConnectionString("NorthwindConnection"));

            services.InstallInfrastractureDependencies(Configuration);
            services.InstallApplicationDependencies(Configuration);

            services.AddAutoMapper();
            services.AddMvc();
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

            applicationLifetime.ApplicationStarted.Register(() => eventLogger.LogInformation("Custom Log AppStarted with path {0}", env.ContentRootPath));
            applicationLifetime.ApplicationStopping.Register(() => eventLogger.LogInformation("Custom Log AppStopped"));

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
                CacheDuration = new System.TimeSpan(1,0,0),
                MaxImages = 10
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
