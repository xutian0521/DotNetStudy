using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreWebApi.Samples.Models;
using Exceptionless;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SkyWalking.AspNetCore;

namespace AspNetCoreWebApi.Samples
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MovieContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("MovieContext"))
                );
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSkyWalking(option =>
            {
                // Application code is showed in sky-walking-ui
                option.ApplicationCode = "AspNetCoreWebApi.Samples";

                //Collector agent_gRPC/grpc service addresses.
                option.DirectServers = "192.168.3.72:11800";

            });
            var appWorkPath = Directory.GetCurrentDirectory();
            //IConfigurationBuilder
            var builder = JsonConfigurationExtensions.AddJsonFile(new ConfigurationBuilder(), appWorkPath + "\\person.json", true, true);
            string text = File.ReadAllText(appWorkPath + "\\person.json");
            services.Configure<Models.Person>(builder.Build().GetSection("root"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.UseExceptionless(Configuration);
        }
    }
}
