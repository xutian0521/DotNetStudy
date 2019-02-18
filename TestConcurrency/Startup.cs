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

            // app.Run(async (context) =>
            // {
            //    int id = await Conn().QueryFirstOrDefaultAsync<int>("SELECT Top 1 id FROM [ws_db].[dbo].[t_hcwtest] WHERE  UserFlag=0");
            //    // 这是flag标记 解决并发插入问题 解决insert不能用 update where 方法
            //    var row = await Conn().ExecuteAsync($"UPDATE [ws_db].[dbo].[t_hcwtest] SET UserFlag=1 WHERE id={id} and UserFlag=0");
            //    if (row > 0)
            //    {
            //        await Conn().ExecuteAsync($"INSERT INTO dbo.t_hcwtest (GUID,UserFlag) VALUES(NEWID(),{id})");
            //    }

            //    await context.Response.WriteAsync("Hello World!");
            // });

            app.Run(async (context) =>
            {
                int id = await Conn().QueryFirstOrDefaultAsync<int>("SELECT Top 1 id FROM [ws_db].[dbo].[t_hcwtest] WHERE  UserFlag=0");
                //1. sqlserver 的 EXISTS SELECT防并发-- 只能放在少了并发
                var row = await Conn().ExecuteAsync($"INSERT INTO dbo.t_hcwtest (GUID,UserFlag) SELECT null,{id} WHERE NOT EXISTS(SELECT 1  FROM dbo.t_hcwtest WHERE UserFlag={id})");
                //2. update where 防并发。 关系型数据库通用的防并发方法-- 利用数据库在更新通一条记录的时候 是原子操作，更新过程：当数据库要更新某一行的时候会在当前行或表 加上排它锁, 这样其他的修改数据库的操作都无法进行。更新完成后释放排它锁
                var row2 = await Conn().ExecuteAsync($"UPDATE [ws_db].[dbo].[t_hcwtest] SET UserFlag=1 WHERE id={id} and UserID=0");
                await context.Response.WriteAsync("Hello World!");
            });

        }
        public static IDbConnection Conn()
        {
            return new SqlConnection("Server=36.7.138.114;UID=sa;PWD=Aa4008577006;database=ws_db;");
        }
    }
}
