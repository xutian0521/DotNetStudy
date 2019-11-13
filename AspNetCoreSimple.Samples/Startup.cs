using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AspNetCoreSimple.Samples
{
    public class Startup
    {
        IConfiguration Configuration;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseStaticFiles();
            //app.UseWelcomePage();

            app.Use(async (content, next) =>
            {
                //await content.Response.WriteAsync("M1");
                logger.LogError("M1-pre");
                await next();
                logger.LogError("M1-next");
            });
            app.Use(async (content, next) =>
            {
                logger.LogWarning("M2-pre");
                await next();
                logger.LogWarning("M2-next");
            });
            app.Run(async (context) =>
            {
                var name = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
                var hosts = this.Configuration["AllowedHosts"];

                await context.Response.WriteAsync(hosts + name);
                logger.LogCritical("hello world!");
            });


            app.UseMvcWithDefaultRoute();
        }
    }
}
