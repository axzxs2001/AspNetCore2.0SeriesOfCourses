using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Dapper;
using Microsoft.Data.Sqlite;
using Working.Models.DataModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Working.Models.Repository;
using System.Data;

namespace Working
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {

            //验证注放
            services.AddAuthentication(opts =>
            {
                opts.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, opt =>
            {
                opt.LoginPath = new PathString("/login");
                opt.Cookie.Path = "/";
            });

            AddRepository(services);
            services.AddMvc();
        }


        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
        /// <summary>
        /// 注放仓储
        /// </summary>
        /// <param name="services">服务容器</param>
        void AddRepository(IServiceCollection services)
        {
            //注放连接字符串
             var connectionString = string.Format(Configuration.GetConnectionString("DefaultConnection"), System.IO.Directory.GetCurrentDirectory());
            services.AddSingleton(connectionString);
            //sqlieconnection注放
            services.AddScoped<IDbConnection, SqliteConnection>();
            //注入用户仓储
            services.AddScoped<IUserRepository, UserRepository>();
            //注入部门仓储
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            //注入工作仓储
            services.AddScoped<IWorkItemRepository, WorkItemRepository>();
            //注放角色仓储
            services.AddScoped<IRoleRepository, RoleRepository>();
        }
    }
}
