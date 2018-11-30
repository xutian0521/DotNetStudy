using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace TestConcurrency
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.Run(async (context) =>
            //{
            //    int id = await Conn().QueryFirstOrDefaultAsync<int>("SELECT Top 1 id FROM [ws_db].[dbo].[t_hcwtest] WHERE  userid=0");
            //    var row = await Conn().ExecuteAsync($"UPDATE [ws_db].[dbo].[t_hcwtest] SET UserID=1 WHERE id={id} and UserID=0");
            //    if (row > 0)
            //    {
            //        await Conn().ExecuteAsync($"INSERT INTO dbo.t_hcwtest (GUID,UserID) VALUES(NEWID(),{id})");
            //    }

            //    await context.Response.WriteAsync("Hello World!");
            //});

            app.Run(async (context) =>
            {
                int id = await Conn().QueryFirstOrDefaultAsync<int>("SELECT Top 1 id FROM [ws_db].[dbo].[t_hcwtest] WHERE  userid=0");
                var row = await Conn().ExecuteAsync($"INSERT INTO dbo.t_hcwtest (GUID,UserID) SELECT null,{id} WHERE NOT EXISTS(SELECT 1  FROM dbo.t_hcwtest WHERE UserID={id})");
                var row2 = await Conn().ExecuteAsync($"UPDATE [ws_db].[dbo].[t_hcwtest] SET UserID=1 WHERE id={id} and UserID=0");
                await context.Response.WriteAsync("Hello World!");
            });

        }
        public static IDbConnection Conn()
        {
            return new SqlConnection("Server=36.7.138.114;UID=sa;PWD=Aa4008577006;database=ws_db;");
        }
    }
}
