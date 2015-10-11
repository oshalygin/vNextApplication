using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Dnx.Runtime;
using Microsoft.Framework.Configuration;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;
using vNextApplication.Models;
using vNextApplication.Services;

namespace vNextApplication
{
    public class Startup
    {
        public static IConfigurationRoot Configuration;

        public Startup(IApplicationEnvironment applicationEnvironment)
        {
            var builder = new ConfigurationBuilder(applicationEnvironment.ApplicationBasePath)
                .AddJsonFile("config.json")
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }


        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddLogging();

            services.AddEntityFramework()
                .AddSqlServer()
                .AddDbContext<WorldContext>();

            services.AddScoped<IMailService, DebugMailService>();
            services.AddTransient<WorldContextSeedData>();
            services.AddScoped<IWorldRepository, WorldRepository>();



            //else
            //{
            //    services.AddScoped<IMailService, MailService>();
            //}
        }

        public void Configure(IApplicationBuilder app, WorldContextSeedData seeder, ILoggerFactory loggerFactory)
        {

            loggerFactory.AddDebug(LogLevel.Warning);

            app.UseStaticFiles();

            app.UseMvc(config =>
            {
                config.MapRoute(
                    name: "Default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new {controller = "App", action = "Index"});
            });

            seeder.EnsureSeedData();
        }
    }
}